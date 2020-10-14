using System;
using System.Drawing;

namespace SnakeApp
{
    internal static class ConsoleExtensions
    {
        public static void Print(this Point point, string symbol = "*")
        {
            Console.SetCursorPosition(point.X, point.Y);

            Console.Write(symbol);
        }

        public static void Clear(this Point point)
        {
            point.Print(" ");
        }
    }
}
