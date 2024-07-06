namespace MazeRace
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerCounter = new System.Windows.Forms.Timer(this.components);
            this.lblCountdown = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnNewGame = new System.Windows.Forms.Button();
            this.panelStartMenu = new System.Windows.Forms.Panel();
            this.lblLevel = new System.Windows.Forms.Label();
            this.ssScore = new System.Windows.Forms.Label();
            this.lblHighScore = new System.Windows.Forms.Label();
            this.lblPause = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelStartMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerCounter
            // 
            this.timerCounter.Interval = 1000;
            this.timerCounter.Tick += new System.EventHandler(this.timerCounter_Tick);
            // 
            // lblCountdown
            // 
            this.lblCountdown.AutoSize = true;
            this.lblCountdown.Font = new System.Drawing.Font("Press Start 2P", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountdown.ForeColor = System.Drawing.Color.Red;
            this.lblCountdown.Location = new System.Drawing.Point(392, 160);
            this.lblCountdown.Name = "lblCountdown";
            this.lblCountdown.Size = new System.Drawing.Size(0, 132);
            this.lblCountdown.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Press Start 2P", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.GreenYellow;
            this.lblTitle.Location = new System.Drawing.Point(119, 227);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(575, 82);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Maze Race";
            // 
            // btnNewGame
            // 
            this.btnNewGame.Font = new System.Drawing.Font("Press Start 2P", 12F);
            this.btnNewGame.ForeColor = System.Drawing.Color.Blue;
            this.btnNewGame.Location = new System.Drawing.Point(258, 364);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(308, 58);
            this.btnNewGame.TabIndex = 1;
            this.btnNewGame.Text = "Start Game";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // panelStartMenu
            // 
            this.panelStartMenu.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panelStartMenu.Controls.Add(this.btnNewGame);
            this.panelStartMenu.Controls.Add(this.lblTitle);
            this.panelStartMenu.Location = new System.Drawing.Point(19, 60);
            this.panelStartMenu.Name = "panelStartMenu";
            this.panelStartMenu.Size = new System.Drawing.Size(776, 543);
            this.panelStartMenu.TabIndex = 2;
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Font = new System.Drawing.Font("Press Start 2P", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblLevel.Location = new System.Drawing.Point(148, 9);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(474, 99);
            this.lblLevel.TabIndex = 3;
            this.lblLevel.Text = "label1";
            // 
            // ssScore
            // 
            this.ssScore.AutoSize = true;
            this.ssScore.Font = new System.Drawing.Font("Press Start 2P", 15F);
            this.ssScore.ForeColor = System.Drawing.Color.Red;
            this.ssScore.Location = new System.Drawing.Point(12, 410);
            this.ssScore.Name = "ssScore";
            this.ssScore.Size = new System.Drawing.Size(198, 41);
            this.ssScore.TabIndex = 4;
            this.ssScore.Text = "label1";
            this.ssScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHighScore
            // 
            this.lblHighScore.AutoSize = true;
            this.lblHighScore.BackColor = System.Drawing.Color.Transparent;
            this.lblHighScore.Font = new System.Drawing.Font("Press Start 2P", 18F);
            this.lblHighScore.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblHighScore.Location = new System.Drawing.Point(292, 373);
            this.lblHighScore.Name = "lblHighScore";
            this.lblHighScore.Size = new System.Drawing.Size(238, 49);
            this.lblHighScore.TabIndex = 3;
            this.lblHighScore.Text = "label1";
            // 
            // lblPause
            // 
            this.lblPause.AutoSize = true;
            this.lblPause.BackColor = System.Drawing.Color.Transparent;
            this.lblPause.Font = new System.Drawing.Font("Press Start 2P", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPause.ForeColor = System.Drawing.Color.Red;
            this.lblPause.Location = new System.Drawing.Point(777, 9);
            this.lblPause.Name = "lblPause";
            this.lblPause.Size = new System.Drawing.Size(132, 27);
            this.lblPause.TabIndex = 5;
            this.lblPause.Text = "Paused";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Image = global::MazeRace.Properties.Resources.info_icon_50_removebg;
            this.lblInfo.Location = new System.Drawing.Point(13, 9);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(35, 32);
            this.lblInfo.TabIndex = 6;
            this.lblInfo.Text = "   ";
            this.toolTip1.SetToolTip(this.lblInfo, "Controls:\r\nP - Pause\r\nO - Reset\r\nW,A,S,D - move");
       
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(921, 450);
            this.Controls.Add(this.panelStartMenu);
            this.Controls.Add(this.lblCountdown);
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.ssScore);
            this.Controls.Add(this.lblHighScore);
            this.Controls.Add(this.lblPause);
            this.Controls.Add(this.lblInfo);
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.panelStartMenu.ResumeLayout(false);
            this.panelStartMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timerCounter;
        private System.Windows.Forms.Label lblCountdown;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnNewGame;
        private System.Windows.Forms.Panel panelStartMenu;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label ssScore;
        private System.Windows.Forms.Label lblHighScore;
        private System.Windows.Forms.Label lblPause;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

