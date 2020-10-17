using System;
using System.Collections.Generic;
using System.Drawing;

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

        public bool CalculateNextPosition(Point applePosition)
        {
            var point = new Point(HeadPosition.X, HeadPosition.Y);

            if (HeadPosition.X < applePosition.X)
            {
                point.X = point.X + 1 == Console.WindowWidth ? 0 : point.X + 1;
            }
            else if (HeadPosition.X > applePosition.X)
            {
                point.X = point.X - 1 == -1 ? Console.WindowWidth - 1 : point.X - 1;
            }
            else if (HeadPosition.Y > applePosition.Y)
            {
                point.Y = point.Y - 1 == -1 ? Console.WindowHeight - 1 : point.Y - 1;
            }
            else if (HeadPosition.Y < applePosition.Y)
            {
                point.Y = point.Y + 1 == Console.WindowHeight ? 0 : point.Y + 1;
            }

            return Print(point, applePosition);
        }

        private bool Print(Point printPoint, Point targetPoint)
        {
            ClearLastPoint();

            if (HasIntercection(printPoint))
            {
                throw new Exception();
            }

            printPoint.Print();

            positions.Add(printPoint);

            var hasInterceptions = HasIntercection(targetPoint);

            length = hasInterceptions ? length + 1 : length;

            return hasInterceptions;
        }

        private Point FindPoint(Point point)
        {
            var intersectionPoint = FindIntersectionPoint(point);

            if (intersectionPoint != null)
            {

            }

            return point;
        }

        private Point? FindIntersectionPoint(Point point)
        {
            foreach (var position in positions)
            {
                if (position.X == point.X && position.Y == point.Y)
                {
                    return position;
                }
            }

            return null;
        }

        private bool HasIntercection(Point point)
        {
            return FindIntersectionPoint(point) != null;
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
