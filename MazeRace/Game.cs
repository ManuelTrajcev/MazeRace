using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeRace
{
    public class Game
    {
        public Player Player;
        public Player PC;
        public Point Finish;
        public int Level;
        public int PCSpeed;
        public List<Point> coins = new List<Point>();
        public int[,] maze;
        public bool isDisabled = false;
        public static MazeGenerator MazeGenerator = new MazeGenerator();

        public Game()
        {
            PCSpeed = 1050;
            Player = new Player(new Point(1, 1));
            PC = new Player(new Point(1, 1));
            Level = 0;
        }

        public void checkMovement(Point newPosition, bool isPC)
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
                  
                    isDisabled = true;
                    DialogResult result = MessageBox.Show("Player reached the finish line!", "Finish Line", MessageBoxButtons.OK);
                    if (result == DialogResult.OK)
                    {
                        StartNextLevel();
                        return;
                    }

                }
                Player.Move(newPosition);
            
            }
        }

        public void StartNextLevel()
        {
           Level++;
            
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
            isDisabled = false;
        }
    }
}
