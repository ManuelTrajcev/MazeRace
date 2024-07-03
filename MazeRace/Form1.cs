using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeRace
{
    public partial class Form1 : Form
    {
        private Player Player;
        private Player PC;
        private Point Finish;
        private int[,] maze;
        public Form1()
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            maze = new int[,]
             {
                { 1, 1, 1, 1, 1 },
                { 1, 0, 2, 0, 1 },
                { 1, 0, 1, 3, 1 },
                { 1, 2, 0, 0, 1 },
                { 1, 1, 1, 1, 1 }
             };
            Player = new Player(new Point(1, 1));
            PC = new Player(new Point(3, 3));
            Finish = new Point(3, 2);
            System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += gameTimer_Tick;
            gameTimer.Start();
            ssScore.Text = $"Player: {Player.Score}    Computer: {PC.Score}";
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            Point bestMove = PC.Position;
            int bestValue = int.MinValue;

            List<Point> possibleMoves = GetPossibleMoves(PC.Position);
            foreach (var move in possibleMoves)
            {
                int moveValue = Minimax(move, 3, false, int.MinValue, int.MaxValue);
                if (moveValue > bestValue)
                {
                    bestValue = moveValue;
                    bestMove = move;
                }
            }

            checkMovement(bestMove, true);
            Invalidate();
        }

        private List<Point> GetPossibleMoves(Point position)
        {
            List<Point> moves = new List<Point>();
            Point[] directions = new Point[]
            {
                new Point(0, 1),  // Down
                new Point(-1, 0), // Left
                new Point(-1, 0), // Left
                new Point(-1, 0), // Left
                new Point(1, 0)   // Right
            };

            foreach (var direction in directions)
            {
                Point newPos = new Point(position.X + direction.X, position.Y + direction.Y);
                if (maze[newPos.Y, newPos.X] != 1)
                {
                    moves.Add(newPos);
                }
            }

            return moves;
        }

        private int Evaluate(Point position)
        {
            return Math.Abs(position.X - Finish.X) + Math.Abs(position.Y - Finish.Y);   // Manhattan distance 
        }

        private int Minimax(Point position, int depth, bool isMaximizingPlayer, int alpha, int beta)
        {
            if (depth == 0 || maze[position.Y, position.X] == 3)
            {
                return Evaluate(position);
            }

            List<Point> possibleMoves = GetPossibleMoves(position);
            if (isMaximizingPlayer)
            {
                int maxEval = int.MinValue;
                foreach (var move in possibleMoves)
                {
                    int eval = Minimax(move, depth - 1, false, alpha, beta);
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                foreach (var move in possibleMoves)
                {
                    int eval = Minimax(move, depth - 1, true, alpha, beta);
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return minEval;
            }
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
            if (maze[newPosition.Y, newPosition.X] == 1)
            {
                return;
            }
            if (isPC)
            {
                if (maze[newPosition.Y, newPosition.X] == 2)
                {
                    PC.UpdateScore(10);
                    maze[newPosition.Y, newPosition.X] = 0;
                }
                else if (maze[newPosition.Y, newPosition.X] == 3)
                {
                    PC.UpdateScore(10);
                    PC.Move(newPosition);
                    Invalidate();
                    MessageBox.Show("You reached the finish line! Your score: " + Player.Score);
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
                }
                else if (maze[newPosition.Y, newPosition.X] == 3)
                {

                    Player.UpdateScore(10);
                    Player.Move(newPosition);
                    Invalidate();
                    MessageBox.Show("You reached the finish line! Your score: " + Player.Score);
                }
                Player.Move(newPosition);
                UpdateScores();
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            int cellSize = 20;
            for (int y = 0; y < maze.GetLength(0); y++)
            {
                for (int x = 0; x < maze.GetLength(1); x++)
                {
                    Brush brush;
                    switch (maze[y, x])
                    {
                        case 1:
                            brush = Brushes.Black;  //Wall
                            break;
                        case 2:
                            brush = Brushes.Gold;   //Coin
                            break;
                        case 3:
                            brush = Brushes.Green;  //Finnish
                            break;
                        default:
                            brush = Brushes.White;  //Path
                            break;
                    }
                    g.FillRectangle(brush, x * cellSize, y * cellSize, cellSize, cellSize);
                    g.DrawRectangle(Pens.Black, x * cellSize, y * cellSize, cellSize, cellSize);
                }
            }

            // Players
            g.FillEllipse(Brushes.Blue, Player.Position.X * cellSize, Player.Position.Y * cellSize, cellSize, cellSize);
            g.FillEllipse(Brushes.Red, PC.Position.X * cellSize, PC.Position.Y * cellSize, cellSize, cellSize);
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
