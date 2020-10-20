using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

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

        public bool Print(Point applePosition)
        {
            var direction = GetDirection(applePosition);

            // var point = GetNextPointFromDirection(direction);

            var point = FindNextValidPoint(direction, applePosition);

            Print(point, applePosition, direction);

            return Print(point, applePosition);
        }

        private bool Print(Point printPoint, Point applePosition)
        {
            ClearLastPoint();

            if (HasIntercection(printPoint))
            {
                // throw new Exception();
            }

            printPoint.Print();

            positions.Add(printPoint);

            var hasInterceptions = HasIntercection(applePosition);

            length = hasInterceptions ? length + 1 : length;

            Console.ReadKey();

            return hasInterceptions;
        }

        private Point FindNextValidPoint(Direction direction, Point applePosition)
        {
            var startPoint = new Point(HeadPosition.X, HeadPosition.Y);
            var searchPoint = new Point();
            var hasRoute = true;

            if (direction.HasFlag(Direction.Down))
            {
                while (startPoint.Y <= Console.WindowHeight)
                {
                    hasRoute = true;
                    if (IsRightFree(startPoint, applePosition, ref searchPoint))
                    {
                        break;
                    }
                    hasRoute = false;
                    ++startPoint.Y;
                }

                if (!hasRoute)
                {
                    startPoint = new Point(HeadPosition.X, HeadPosition.Y);
                    searchPoint = new Point();

                    while (startPoint.Y <= Console.WindowHeight)
                    {
                        IsLeftFree(startPoint, applePosition, ref searchPoint);
                        ++startPoint.Y;
                    }
                }
            }
            else if (direction.HasFlag(Direction.Up))
            {
                while (startPoint.Y >= 0)
                {
                    hasRoute = true;
                    if (IsRightFree(startPoint, applePosition, ref searchPoint))
                    {
                        break;
                    }
                    hasRoute = false;
                    --startPoint.Y;
                }

                if (!hasRoute)
                {
                    startPoint = new Point(HeadPosition.X, HeadPosition.Y);
                    searchPoint = new Point();

                    while (startPoint.Y >= 0)
                    {
                        IsLeftFree(startPoint, applePosition, ref searchPoint);
                        --startPoint.Y;
                    }
                }
            }
            else if (direction.HasFlag(Direction.Right))
            {
                while (startPoint.X <= Console.WindowWidth)
                {
                    hasRoute = true;
                    if (IsUpFree(startPoint, applePosition, ref searchPoint))
                    {
                        break;
                    }
                    hasRoute = false;
                    ++startPoint.X;
                }

                if (!hasRoute)
                {
                    startPoint = new Point(HeadPosition.X, HeadPosition.Y);
                    searchPoint = new Point();

                    while (startPoint.X <= Console.WindowWidth)
                    {
                        IsDownFree(startPoint, applePosition, ref searchPoint);
                        ++startPoint.X;
                    }
                }
            }
            else if (direction.HasFlag(Direction.Left))
            {
                while (startPoint.X >= 0)
                {
                    hasRoute = true;
                    if (!IsUpFree(startPoint, applePosition, ref searchPoint))
                    {
                        break;
                    }
                    hasRoute = false;
                    --startPoint.X;
                }

                if (!hasRoute)
                {
                    startPoint = new Point(HeadPosition.X, HeadPosition.Y);
                    searchPoint = new Point();

                    while (startPoint.X >= 0)
                    {
                        IsDownFree(startPoint, applePosition, ref searchPoint);
                        --startPoint.X;
                    }
                }
            }

            return searchPoint;
        }

        private bool IsRightFree(Point point, Point applePosition, ref Point freePoint)
        {
            var stop = false;
            var originalPoint = new Point(point.X, point.Y);

            var mostRightPoint = positions.Count == 0 ? Console.WindowWidth : positions.Max(p => p.X);
            mostRightPoint = point.X > mostRightPoint ? Console.WindowWidth : mostRightPoint;

            while (originalPoint.Y < applePosition.Y && originalPoint.X <= mostRightPoint)
            {
                originalPoint.Y++;
                stop = false;
                if (HasIntercection(originalPoint))
                {
                    ++originalPoint.X;
                    originalPoint.Y = point.Y;
                    stop = true;
                }
            }
            freePoint = originalPoint;
            return !stop;
        }

        private bool IsDownFree(Point point, Point applePosition, ref Point searchPoint)
        {
            var originalPoint = new Point(point.X, point.Y);
            var stop = false;

            int mostDownPosition = positions.Count == 0 ? 0 : positions.Max(x => x.Y);
            mostDownPosition = HeadPosition.Y < mostDownPosition ? mostDownPosition : Console.WindowHeight;

            while (++originalPoint.X < applePosition.X && originalPoint.Y >= mostDownPosition)
            {
                stop = false;
                if (HasIntercection(originalPoint))
                {
                    ++originalPoint.Y;
                    originalPoint.X = point.X;

                    stop = true;
                }
            }
            searchPoint = originalPoint;
            return !stop;

        }

        private bool IsUpFree(Point point, Point applePosition, ref Point searchPoint)
        {
            var originalPoint = new Point(point.X, point.Y);
            var stop = false;
            while (++originalPoint.X < applePosition.X && originalPoint.Y >= 0)
            {
                stop = false;
                if (HasIntercection(originalPoint))
                {
                    --originalPoint.Y;
                    originalPoint.X = point.X;

                    stop = true;
                }
            }
            searchPoint = originalPoint;
            return !stop;
        }

        private bool IsLeftFree(Point point, Point applePosition, ref Point freePoint)
        {
            var originalPoint = new Point(point.X, point.Y);
            var stop = false;
            while (++originalPoint.Y < applePosition.Y && originalPoint.X >= 0)
            {
                stop = false;

                if (HasIntercection(originalPoint))
                {
                    --originalPoint.X;
                    originalPoint.Y = point.Y;

                    stop = true;
                }
            }
            freePoint = originalPoint;
            return !stop;
        }

        private void Print(Point targetPoint, Point applePosition, Direction direction)
        {
            if (direction.HasFlag(Direction.Down))
            {
                for (int i = HeadPosition.Y; i <= targetPoint.Y; i++)
                {
                    Print(new Point(HeadPosition.X, i), applePosition);

                    Console.ReadKey();
                }
            }
            else if (direction.HasFlag(Direction.Right))
            {
                for (int i = HeadPosition.X; i <= targetPoint.X; i++)
                {
                    Print(new Point(i, HeadPosition.Y), applePosition);

                    Console.ReadKey();
                }
            }
        }

        private Point? FindIntersectionPoint(Point point)
        {
            if (positions.Count == 1)
            {
                return null;
            }

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

        private Direction GetDirection(Point applePosition)
        {
            var direction = Direction.None;

            if (HeadPosition.X < applePosition.X)
            {
                direction = Direction.Right;
            }
            else if (HeadPosition.X > applePosition.X)
            {
                direction = Direction.Left;
            }

            if (HeadPosition.Y > applePosition.Y)
            {
                direction |= Direction.Up;
            }
            else if (HeadPosition.Y < applePosition.Y)
            {
                direction |= Direction.Down;
            }

            return direction;
        }

        private Point GetNextPointFromDirection(Direction direction)
        {
            var point = new Point(HeadPosition.X, HeadPosition.Y);

            if (direction.HasFlag(Direction.Right))
            {
                point.X = point.X + 1 == Console.WindowWidth ? 0 : point.X + 1;
            }
            else if (direction.HasFlag(Direction.Left))
            {
                point.X = point.X - 1 == -1 ? Console.WindowWidth - 1 : point.X - 1;
            }
            else if (direction.HasFlag(Direction.Up))
            {
                point.Y = point.Y - 1 == -1 ? Console.WindowHeight - 1 : point.Y - 1;
            }
            else if (direction.HasFlag(Direction.Down))
            {
                point.Y = point.Y + 1 == Console.WindowHeight ? 0 : point.Y + 1;
            }

            return point;
        }

        [Flags]
        private enum Direction
        {
            None = 0,
            Right = 1,
            Left = 2,
            Up = 4,
            Down = 8
        }
    }
}
