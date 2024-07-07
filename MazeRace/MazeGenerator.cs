using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRace
{
    public class MazeGenerator
    {
        private static readonly Random random = new Random();
        private const int wall = 1;
        private const int path = 0;
        private const int coin = 2;
        private const int finish = 3;
        private const int width = 19;
        private const int height = 19;

        public int[,] GenerateMaze(int numberOfCoins)
        {
            int[,] maze = new int[width, height];
            InitializeMaze(maze);
            GeneratePaths(maze);
            PlaceFinish(maze);
            PlaceCoins(maze, numberOfCoins);
            return maze;
        }

        private void InitializeMaze(int[,] maze)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    maze[y, x] = wall;
                }
            }
        }

        private void GeneratePaths(int[,] maze)
        {
            Stack<Point> stack = new Stack<Point>();
            Point start = new Point(1, 1);
            stack.Push(start);
            maze[start.Y, start.X] = path;

            while (stack.Count > 0)
            {
                Point current = stack.Peek();
                List<Point> neighbors = GetUnvisitedNeighbors(maze, current);

                if (neighbors.Count > 0)
                {
                    Point chosen = neighbors[random.Next(neighbors.Count)];
                    RemoveWall(maze, current, chosen);
                    stack.Push(chosen);
                    maze[chosen.Y, chosen.X] = path;
                }
                else
                {
                    stack.Pop();
                }
            }
        }

        private List<Point> GetUnvisitedNeighbors(int[,] maze, Point p)
        {
            List<Point> neighbors = new List<Point>();

            if (p.Y > 2 && maze[p.Y - 2, p.X] == wall) neighbors.Add(new Point(p.X, p.Y - 2));
            if (p.Y < height - 3 && maze[p.Y + 2, p.X] == wall) neighbors.Add(new Point(p.X, p.Y + 2));
            if (p.X > 2 && maze[p.Y, p.X - 2] == wall) neighbors.Add(new Point(p.X - 2, p.Y));
            if (p.X < width - 3 && maze[p.Y, p.X + 2] == wall) neighbors.Add(new Point(p.X + 2, p.Y));

            return neighbors;
        }

        private void RemoveWall(int[,] maze, Point a, Point b)
        {
            int x = (a.X + b.X) / 2;
            int y = (a.Y + b.Y) / 2;
            maze[y, x] = path;
        }

        private void PlaceFinish(int[,] maze)
        {
            int finishX;
            int finishY;
            while (true)
            {
                 finishX = random.Next(5, width - 5);
                 finishY = random.Next(5, height - 5);
                if(maze[finishX, finishY] == wall) break;
            }
            
            maze[finishY, finishX] = finish;
        }

        private void PlaceCoins(int[,] maze, int numberOfCoins)
        {
            int placedCoins = 0;
            while (placedCoins < numberOfCoins)
            {
                int x = random.Next(1, width - 1);
                int y = random.Next(1, height - 1);
                if (maze[y, x] == path)
                {
                    maze[y, x] = coin;
                    placedCoins++;
                }
            }
        }
    }
}

