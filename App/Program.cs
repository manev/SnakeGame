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
        private static object _drawLock = new object();

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

            apple.Print();

            InitTimer();

            while (currentKey != ConsoleKey.Q) ;
        }

        private static void SetupConsole()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.BufferHeight = Console.WindowHeight = 40;
            Console.BufferWidth = Console.WindowWidth = 85;
            Console.CursorVisible = false;
        }

        private static void InitTimer()
        {
            var timer = new Timer(50);
            timer.Elapsed += Print;
            timer.Enabled = true;
            timer.Start();
        }

        private static void Print(object sender, ElapsedEventArgs args)
        {
            try
            {
                lock (_drawLock)
                {
                    currentKey = Console.KeyAvailable ? Console.ReadKey().Key : currentKey;

                    if (snake.Print(currentKey, apple.Position))
                    {
                        apple.Print(snake.GetPosition());
                    }

                    currentKey = CalculateNextPosition();
                }
            }
            catch (Exception)
            {
                Reset(sender);
            }
        }

        private static void Reset(object sender)
        {
            var timer = sender as Timer;
            timer.Elapsed -= Print;
            timer.Enabled = false;
            timer.Stop();
            timer.Dispose();

            Console.Clear();
            Console.WriteLine("Game Over");
            Console.ReadLine();
            Console.Clear();

            StartGame();
        }

        private static ConsoleKey CalculateNextPosition()
        {
            var snakeHeadPosition = snake.HeadPosition;

            var applePosition = apple.Position;

            // var snakePosition = snake.Position;

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
