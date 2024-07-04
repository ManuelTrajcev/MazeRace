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
        private Player Player;
        private Player PC;
        private Point Finish;
        private int Level;
        private int PCSpeed;
        List<Point> coins = new List<Point>();
        private int[,] maze;
        private Timer gameTimer;
        private bool isDisabled = false;
        private static MazeGenerator MazeGenerator = new MazeGenerator();
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Height = 600;
            this.Width = 600;
            PCSpeed = 1050;
            Player = new Player(new Point(1, 1));
            PC = new Player(new Point(1, 1));
            Level = 0;
            ssScore.Text = $"Player: {Player.Score}    Computer: {PC.Score}";
            gameTimer = new Timer();
            StartNextLevel();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            List<Point> pathToFinish = MazeSolver.FindShortestPath(maze, PC.Position, Finish);

            List<Point> path = pathToFinish;
            if (coins.Count > 0)
            {
                List<Point> pathToClosestCoin = MazeSolver.FindShortestPath(maze, PC.Position, coins[0]);
                coins.ForEach(c =>
                {
                    if (MazeSolver.FindShortestPath(maze, PC.Position, c).Count < pathToClosestCoin.Count)
                    {
                        pathToClosestCoin = MazeSolver.FindShortestPath(maze, PC.Position, c);
                    }
                });
                path = pathToFinish.Count > pathToClosestCoin.Count && pathToClosestCoin.Count > 1 ? pathToClosestCoin : pathToFinish;

            }



            if (path != null && path.Count > 1)
            {
                Point bestMove = path[1];
                checkMovement(bestMove, true);
            }
            Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Point newPosition = Player.Position;
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

            checkMovement(newPosition, false);

        }

        private void checkMovement(Point newPosition, bool isPC)
        {
            if (maze[newPosition.Y, newPosition.X] == 1 || isDisabled)
            {
                return;
            }
            if (isPC)
            {
                if (maze[newPosition.Y, newPosition.X] == 2)
                {
                    PC.UpdateScore(10);
                    maze[newPosition.Y, newPosition.X] = 0;
                    coins.Remove(newPosition);
                }
                else if (maze[newPosition.Y, newPosition.X] == 3)
                {
                    PC.UpdateScore(10);
                    PC.Move(newPosition);
                    Invalidate();
                    isDisabled = true;
                    DialogResult result = MessageBox.Show("PC reached the finish line!", "Finish Line", MessageBoxButtons.OK);
                    if (result == DialogResult.OK)
                    {
                        StartNextLevel();
                        return;
                    }
                    StartNextLevel();
                }
                PC.Move(newPosition);
                UpdateScores();
                Invalidate();
            }
            else
            {
                if (maze[newPosition.Y, newPosition.X] == 2)
                {
                    Player.UpdateScore(10);
                    maze[newPosition.Y, newPosition.X] = 0;
                    coins.Remove(newPosition);
                }
                else if (maze[newPosition.Y, newPosition.X] == 3)
                {

                    Player.UpdateScore(10);
                    Player.Move(newPosition);
                    Invalidate();
                    isDisabled = true;
                    DialogResult result = MessageBox.Show("Player reached the finish line!", "Finish Line", MessageBoxButtons.OK);
                    if (result == DialogResult.OK)
                    {
                        StartNextLevel();
                        return;
                    }

                }
                Player.Move(newPosition);
                UpdateScores();
                Invalidate();
            }
        }

        private void StartNextLevel()
        {
            Level++;
            gameTimer.Stop();
            PC.setPosition(new Point(1, 1));
            Player.setPosition(new Point(1, 1));
            coins = new List<Point>();
            maze = MazeGenerator.GenerateMaze(Level);
            for (int y = 0; y < maze.GetLength(0); y++)
            {
                for (int x = 0; x < maze.GetLength(1); x++)
                {
                    if (maze[y, x] == 2)
                    {
                        coins.Add(new Point(x, y));
                    }
                    else if (maze[y, x] == 3)
                    {
                        Finish = new Point(x, y);
                    }
                }
            }
            gameTimer = new Timer();
            isDisabled = false;
            PCSpeed -= 50;
            gameTimer.Interval = PCSpeed;
            gameTimer.Tick += gameTimer_Tick;
            gameTimer.Start();
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            int cellSize = 20;
            int mazeWidth = maze.GetLength(1) * cellSize;
            int mazeHeight = maze.GetLength(0) * cellSize;

            // Calculate offsets to center the maze
            int offsetX = (ClientSize.Width - mazeWidth) / 2;
            int offsetY = (ClientSize.Height - mazeHeight) / 2;

            for (int y = 0; y < maze.GetLength(0); y++)
            {
                for (int x = 0; x < maze.GetLength(1); x++)
                {
                    Brush brush;
                    switch (maze[y, x])
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
            int playerDrawX = Player.Position.X * cellSize + offsetX;
            int playerDrawY = Player.Position.Y * cellSize + offsetY;
            g.FillEllipse(Brushes.Blue, playerDrawX, playerDrawY, cellSize, cellSize);

            int pcDrawX = PC.Position.X * cellSize + offsetX;
            int pcDrawY = PC.Position.Y * cellSize + offsetY;
            g.FillEllipse(Brushes.Red, pcDrawX, pcDrawY, cellSize, cellSize);
        }

        private void UpdateScores()
        {
            ssScore.Text = $"Player: {Player.Score}    Computer: {PC.Score}";
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
