using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SnakeApp
{
    internal class Snake
    {
        private int length = 50;

        private List<Point> positions = new List<Point>();

        public Point HeadPosition
        {
            get
            {
                return positions?.Count > 0 ? positions[positions.Count - 1] : Point.Empty;
            }
        }

        public IEnumerable<Point> Position
        {
            get
            {
                return positions.ToArray();
            }
        }

        public bool Print(ConsoleKey key, Point targetPoint)
        {
            ClearLastPoint();

            var point = GetNextPoint(key);

            if (positions.Any(position => position.X == point.X && position.Y == point.Y))
            {
                throw new Exception();
            }

            point.Print();

            positions.Add(point);

            var hasInterceptions = positions.Any(position => position.X == targetPoint.X && position.Y == targetPoint.Y);

            length = hasInterceptions ? length + 1 : length;

            return hasInterceptions;
        }

        private Point GetNextPoint(ConsoleKey key)
        {
            var point = new Point(HeadPosition.X, HeadPosition.Y);

            switch (key)
            {
                case ConsoleKey.DownArrow:
                    point.Y = point.Y + 1 == Console.WindowHeight ? 0 : point.Y + 1;
                    break;

                case ConsoleKey.UpArrow:
                    point.Y = point.Y - 1 == -1 ? Console.WindowHeight - 1 : point.Y - 1;
                    break;

                case ConsoleKey.LeftArrow:
                    point.X = point.X - 1 == -1 ? Console.WindowWidth - 1 : point.X - 1;
                    break;

                default:
                    point.X = point.X + 1 == Console.WindowWidth ? 0 : point.X + 1;
                    break;
            }


            return point;
        }

        private void ClearLastPoint()
        {
            if (positions.Count == length)
            {
                var first = positions[0];

                first.Clear();

                positions.Remove(first);
            }
        }
    }
}
