﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SnakeApp
{
    internal class Snake
    {
        private int length = 20;

        private List<Point> positions = new List<Point>();

        public bool Draw(ConsoleKey key, Point targetPoint)
        {
            ClearLastPoint();

            GetNextPoint(key).Draw();

            var hasInterceptions = positions.Any(position => position.X == targetPoint.X && position.Y == targetPoint.Y);

            length = hasInterceptions ? length + 1 : length;

            return hasInterceptions;
        }

        public IList<Point> GetPosition()
        {
            return positions;
        }

        public Point GetHeadPosition()
        {
            return positions[positions.Count - 1];
        }

        private Point GetNextPoint(ConsoleKey key)
        {
            var lastPoint = positions.Count == 0 ? new Point() : positions[positions.Count - 1];

            var point = new Point(lastPoint.X, lastPoint.Y);

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

            positions.Add(point);

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