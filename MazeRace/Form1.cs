using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MazeRace
{
    public partial class Form1 : Form
    {
        private int Countdown;
        private Timer gameTimer;
        private bool isDisabled = true;
        private static MazeGenerator MazeGenerator = new MazeGenerator();
        private Game Game;
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Height = 600;
            this.Width = 600;
            Countdown = 4;
            Game = new Game();
            Game.OnLevelCompleted += StartNextLevel;
            ssScore.Text = $"Player: {Game.Player.Score}    Computer: {Game.PC.Score}";
            gameTimer = new Timer();
            StartNextLevel();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            List<Point> pathToFinish = MazeSolver.FindShortestPath(Game.maze, Game.PC.Position, Game.Finish);
            List<Point> path = pathToFinish;
            if (Game.coins.Count > 0)
            {
                List<Point> pathToClosestCoin = MazeSolver.FindShortestPath(Game.maze, Game.PC.Position, Game.coins[0]);
                Game.coins.ForEach(c =>
                {
                    if (MazeSolver.FindShortestPath(Game.maze, Game.PC.Position, c).Count < pathToClosestCoin.Count)
                    {
                        pathToClosestCoin = MazeSolver.FindShortestPath(Game.maze, Game.PC.Position, c);
                    }
                });
                path = pathToFinish.Count > pathToClosestCoin.Count && pathToClosestCoin.Count > 1 ? pathToClosestCoin : pathToFinish;
            }

            if (path != null && path.Count > 1)
            {
                Point bestMove = path[1];
                Game.checkMovement(bestMove, true);
            }
            Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isDisabled)
            {
                Point newPosition = Game.Player.Position;
                switch (e.KeyCode)
                {
                    case Keys.W:
                        newPosition.Y -= 1;
                        break;
                    case Keys.A:
                        newPosition.X -= 1;
                        break;
                    case Keys.S:
                        newPosition.Y += 1;
                        break;
                    case Keys.D:
                        newPosition.X += 1;
                        break;
                }

                Game.checkMovement(newPosition, false);
                UpdateScores();
                Invalidate();
            }
           
        }

        private void checkMovement(Point newPosition, bool isPC)
        {
                Game.Player.Move(newPosition);
                UpdateScores();
                Invalidate();
        }

        public void StartNextLevel()
        {
            gameTimer.Stop();
            isDisabled = true;
            timerCounter.Start();
            gameTimer = new Timer();
         
            Game.PCSpeed -= 50;
            gameTimer.Interval = Game.PCSpeed;
            Game.StartNextLevel();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            int cellSize = 20;
            int mazeWidth = Game.maze.GetLength(1) * cellSize;
            int mazeHeight = Game.maze.GetLength(0) * cellSize;

            // Calculate offsets to center the maze
            int offsetX = (ClientSize.Width - mazeWidth) / 2;
            int offsetY = (ClientSize.Height - mazeHeight) / 2;

            for (int y = 0; y < Game.maze.GetLength(0); y++)
            {
                for (int x = 0; x < Game.maze.GetLength(1); x++)
                {
                    Brush brush;
                    switch (Game.maze[y, x])
                    {
                        case 1:
                            brush = Brushes.Black;  // Wall
                            break;
                        case 2:
                            brush = Brushes.Gold;   // Coin
                            break;
                        case 3:
                            brush = Brushes.Green;  // Finish
                            break;
                        default:
                            brush = Brushes.White;  // Path
                            break;
                    }
                    int drawX = x * cellSize + offsetX;
                    int drawY = y * cellSize + offsetY;
                    g.FillRectangle(brush, drawX, drawY, cellSize, cellSize);
                    g.DrawRectangle(Pens.Black, drawX, drawY, cellSize, cellSize);
                }
            }

            // Draw players
            int playerDrawX = Game.Player.Position.X * cellSize + offsetX;
            int playerDrawY = Game.Player.Position.Y * cellSize + offsetY;
            g.FillEllipse(Brushes.Blue, playerDrawX, playerDrawY, cellSize, cellSize);

            int pcDrawX = Game.PC.Position.X * cellSize + offsetX;
            int pcDrawY = Game.PC.Position.Y * cellSize + offsetY;
            g.FillEllipse(Brushes.Red, pcDrawX, pcDrawY, cellSize, cellSize);
        }

        private void UpdateScores()
        {
            ssScore.Text = $"Player: {Game.Player.Score}    Computer: {Game.PC.Score}";
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timerCounter_Tick(object sender, EventArgs e)
        {
            Countdown--;
            lblCountdown.Text = Countdown.ToString();
            Game.isDisabled = true;
            if (Countdown == 0)
            {
                lblCountdown.Text = "";
                Countdown = 4;
                timerCounter.Stop();
                Game.isDisabled = false;
                isDisabled = false;

                gameTimer.Tick += gameTimer_Tick;
                gameTimer.Start();
            }
            
        }
    }
}
