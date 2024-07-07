using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using WMPLib;

namespace MazeRace
{
    public partial class MazeRace : Form
    {
        private PrivateFontCollection arcadeFontCollection;
        private int Countdown;
        private Timer gameTimer;
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        private bool isDisabled { get; set; }
        private static MazeGenerator MazeGenerator = new MazeGenerator();
        private Game Game = null;
        public int Highscore = 0;
        private bool isMusicOn = true;

        public MazeRace()
        {
            InitializeComponent();
            StartMusic();
            //LoadArcadeFont();
            //ApplyArcadeFont(this.Controls);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            int formWidth = this.Width;
            int formHeight = this.Height;
            isDisabled = true;
            int posX = (screenWidth - formWidth) / 2;
            int posY = (screenHeight - formHeight) / 4;

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(posX, posY);

            this.DoubleBuffered = true;
            this.Height = 600;
            this.Width = 600;
            AdjustPanel();
        }

        private void StartMusic()
        {
            player.SoundLocation = "music.wav";
            player.LoadCompleted += Player_LoadCompleted;
            player.LoadAsync();
        }
        private void AdjustPanel()
        {
            panelStartMenu.Width = this.Width;
            panelStartMenu.Height = this.Height;
            panelStartMenu.Left = 0;
            panelStartMenu.Top = 0;
            lblTitle.Left = (panelStartMenu.Width - lblTitle.Width) / 2;
            btnNewGame.Left = (panelStartMenu.Width - btnNewGame.Width) / 2;
            lblCountdown.BackColor = Color.Transparent;
            lblCountdown.Top = (this.Height - lblCountdown.Height) / 2;
            lblCountdown.Left = (this.Width - lblCountdown.Height) / 2;
            lblLevel.Left = (this.Width - lblLevel.Width) / 2;
            lblLevel.Top = 20;
            ssScore.Width = this.Width;
            ssScore.Left = (this.Width - ssScore.Width) / 2;
            ssScore.Top = this.Height - 3 * ssScore.Height;
            lblHighScore.Top = this.Height - (int)(4.5 * ssScore.Height);
            lblHighScore.Left = (this.Width - lblHighScore.Width) / 2;
            lblPause.Top = 10;
            lblPause.Left = this.Width - lblPause.Width - 27;
            lblSound.Left = this.Width - lblPause.Width - 7;
            lblSound.Top = 30;
            lblInfo.Left = 10;
            lblInfo.Top = 10;
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (!isDisabled)
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
            }
            Invalidate();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P)
            {
                if (isDisabled)
                {
                    ResetGame();
                }
                else
                {
                    PauseGame();
                }
            }
            if (e.KeyCode == Keys.M)
            {
                if (isMusicOn)
                {
                    player.Stop();
                    lblSound.Image = Properties.Resources.off__1_;
                }
                else
                {
                    player.Play();
                    lblSound.Image = Properties.Resources.on__1_;
                }
                isMusicOn = !isMusicOn;
            }

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
            if (Game.Player.Score > Highscore)
            {
                Highscore = Game.Player.Score;
            }
            lblHighScore.Text = $"Highscore: {Highscore}";
            gameTimer.Stop();
            isDisabled = true;
            timerCounter.Start();
            gameTimer = new Timer();
            lblLevel.Text = $"Level {Game.Level}";
            gameTimer.Interval = Game.PCSpeed;
            Game.StartNextLevel();

            if (Game.Level == 12)
            {
                DialogResult result = MessageBox.Show("You have passed all 10 level! Start new game?", "Geme Over", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    StartNewGame();
                }
                else
                {
                    this.Close();
                }
            }
            AdjustPanel();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Game == null)
            {
                return;
            }
            base.OnPaint(e);
            Graphics g = e.Graphics;

            int cellSize = 20;
            int mazeWidth = Game.maze.GetLength(1) * cellSize;
            int mazeHeight = Game.maze.GetLength(0) * cellSize;
            int offsetX = (ClientSize.Width - mazeWidth) / 2;
            int offsetY = (ClientSize.Height - mazeHeight) / 2;

            for (int y = 0; y < Game.maze.GetLength(0); y++)
            {
                for (int x = 0; x < Game.maze.GetLength(1); x++)
                {
                    Brush brush;
                    int drawX = x * cellSize + offsetX;
                    int drawY = y * cellSize + offsetY;

                    switch (Game.maze[y, x])
                    {
                        case 1:
                            brush = Brushes.Black;  // Wall
                            g.FillRectangle(brush, drawX, drawY, cellSize, cellSize);
                            break;
                        case 2:
                            brush = Brushes.Gold;   // Coin
                            g.FillEllipse(brush, drawX, drawY, cellSize, cellSize);
                            break;
                        case 3:
                            brush = Brushes.Green;  // Finish
                            g.FillRectangle(brush, drawX, drawY, cellSize, cellSize);
                            break;
                        default:
                            brush = Brushes.White;  // Path
                            g.FillRectangle(brush, drawX, drawY, cellSize, cellSize);
                            break;
                    }
                    g.DrawRectangle(Pens.Black, drawX, drawY, cellSize, cellSize);
                }
            }

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

        private void timerCounter_Tick(object sender, EventArgs e)
        {
            lblPause.Text = "Paused";
            lblPause.ForeColor = Color.Red;
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
                lblPause.Text = "Playing";
                lblPause.ForeColor = Color.DarkGreen;
            }
        }
        private void LoadArcadeFont()
        {
            arcadeFontCollection = new PrivateFontCollection();
            string fontPath = Path.Combine(Application.StartupPath, "Fonts", "PressStart2P-Regular.ttf");

            MessageBox.Show("Font path: " + fontPath); // Add this line to debug

            arcadeFontCollection.AddFontFile(fontPath);

            Font arcadeFont = new Font(arcadeFontCollection.Families[0], 12, FontStyle.Regular);

            ssScore.Font = new Font(arcadeFont, FontStyle.Regular);
            lblCountdown.Font = new Font(arcadeFont, FontStyle.Regular);
            btnNewGame.Font = new Font(arcadeFont, FontStyle.Regular);
        }

        private void ApplyArcadeFont(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                control.Font = new Font(arcadeFontCollection.Families[0], control.Font.Size);
                if (control.HasChildren)
                {
                    ApplyArcadeFont(control.Controls);
                }
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private void StartNewGame()
        {
            AdjustPanel();
            Countdown = 4;
            Game = new Game();
            Game.OnLevelCompleted += StartNextLevel;
            ssScore.Text = $"Player: {Game.Player.Score}    Computer: {Game.PC.Score}";
            gameTimer = new Timer();
            StartNextLevel();
            panelStartMenu.Visible = false;
            this.Focus();
        }

        private void PauseGame()
        {
            isDisabled = true;
            Game.isDisabled = true;
            lblPause.Text = "Paused";
            lblPause.ForeColor = Color.Red;
        }

        private void ResetGame()
        {
            isDisabled = false;
            Game.isDisabled = false;
            lblPause.Text = "Playing";
            lblPause.ForeColor = Color.DarkGreen;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }
        private void Player_LoadCompleted(object sender, EventArgs e)
        {
            player.PlayLooping();
        }
    }
}
