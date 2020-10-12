using System;
using System.Drawing;
using System.Text;
using System.Timers;

using SnakeApp;

namespace SnakeProgram
{
    class Program
    {
        private static Snake snake;
        private static Apple apple;
        private static ConsoleKey currentKey;

        static void Main()
        {
            SetupConsole();

            StartGame();
        }

        private static void StartGame()
        {
            Console.Clear();

            currentKey = ConsoleKey.RightArrow;

            snake = new Snake();
            apple = new Apple();

            apple.DrawNew();

            InitTimer();

            while (currentKey != ConsoleKey.Q) ;
        }

        private static void SetupConsole()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.BufferHeight = Console.WindowHeight = 30;
            Console.BufferWidth = Console.WindowWidth = 75;
            Console.CursorVisible = false;
        }

        private static void InitTimer()
        {
            var timer = new Timer(100);
            timer.Elapsed += OnTimerElapsed;
            timer.Enabled = true;
            timer.Start();
        }

        private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            currentKey = Console.KeyAvailable ? Console.ReadKey().Key : currentKey;

            if (snake.Draw(currentKey, apple.Position))
            {
                apple.DrawNew(snake.GetPosition());
            }

            currentKey = CalculateNextPosition(snake.GetHeadPosition(), apple.Position);
        }

        private static ConsoleKey CalculateNextPosition(Point snakeHeadPosition, Point applePosition)
        {
            if (snakeHeadPosition.X < applePosition.X)
            {
                return ConsoleKey.RightArrow;
            }
            else if (snakeHeadPosition.X > applePosition.X)
            {
                return ConsoleKey.LeftArrow;
            }
            else if (snakeHeadPosition.Y > applePosition.Y)
            {
                return ConsoleKey.UpArrow;
            }
            else if (snakeHeadPosition.Y < applePosition.Y)
            {
                return ConsoleKey.DownArrow;
            }

            return ConsoleKey.NoName;
        }
    }
}
