using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRace
{
    public class Player
    {
        public Point Position { get; set; }
        public int Score { get; set; }

        public Player(Point position)
        {
            Position = position;
            Score = 0;
        }
        public void Move(Point newPosition)
        {
            Position = newPosition;
        }
        public void UpdateScore(int score)
        {
            Score += score;
        }
    }
}
