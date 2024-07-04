using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRace
{
    public class MazeSolver
    {
        private static readonly int[] rowMoves = { -1, 1, 0, 0 };
        private static readonly int[] colMoves = { 0, 0, -1, 1 };

        public static List<Point> FindShortestPath(int[,] maze, Point start, Point end)
        {
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);
            bool[,] visited = new bool[rows, cols];
            Point[,] previous = new Point[rows, cols];

            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(start);
            visited[start.Y, start.X] = true;

            while (queue.Count > 0)
            {
                Point current = queue.Dequeue();

                if (current == end)
                    return ReconstructPath(previous, start, end);

                for (int i = 0; i < 4; i++)
                {
                    int newRow = current.Y + rowMoves[i];
                    int newCol = current.X + colMoves[i];

                    if (IsValidMove(maze, newRow, newCol, visited))
                    {
                        queue.Enqueue(new Point(newCol, newRow));
                        visited[newRow, newCol] = true;
                        previous[newRow, newCol] = current;
                    }
                }
            }

            return new List<Point>(); // No path found
        }

        private static bool IsValidMove(int[,] maze, int row, int col, bool[,] visited)
        {
            return row >= 0 && row < maze.GetLength(0) && col >= 0 && col < maze.GetLength(1)
                   && maze[row, col] != 1 && !visited[row, col];
        }

        private static List<Point> ReconstructPath(Point[,] previous, Point start, Point end)
        {
            List<Point> path = new List<Point>();
            for (Point at = end; at != start; at = previous[at.Y, at.X])
            {
                path.Add(at);
            }
            path.Add(start);
            path.Reverse();
            return path;
        }
    }
}
