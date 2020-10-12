using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SnakeApp
{
    internal class Apple
    {
        private static Random random = new Random();
        public Point Position { get; set; }

        public void DrawNew(IList<Point> snakePosition = null)
        {
            Position = GeneratePosition(snakePosition);

            Position.Draw();
        }

        private Point GeneratePosition(IEnumerable<Point> positions)
        {
            var point = new Point();

            do
            {
                point.X = random.Next(0, Console.WindowWidth);
                point.Y = random.Next(0, Console.WindowHeight);
            }
            while (positions != null && positions.Any(position => position.X == point.X && position.Y == point.Y));

            return point;
        }
    }
}
