using System;
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
            Console.BufferHeight = Console.WindowHeight = 20;
            Console.BufferWidth = Console.WindowWidth = 45;
            Console.CursorVisible = false;
        }

        private static void InitTimer()
        {
            var timer = new Timer(100);
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

                    if (snake.CalculateNextPosition(apple.Position))
                    {
                        apple.Print(snake.Position);
                    }
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

            // Console.Clear();
            // Console.WriteLine("Game Over");
            Console.ReadLine();
            // Console.Clear();

            StartGame();
        }
    }
}
