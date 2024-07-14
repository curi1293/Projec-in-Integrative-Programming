using System;
using System.Threading;

namespace SchoolSystem.Utilities
{
    public class Loading
    {
        public static void DisplayLoading(string message, int delay = 100)
        {
            Console.Clear();
            int consoleWidth = Console.WindowWidth;
            int messageLength = message.Length;
            int spaces = (consoleWidth / 2) - (messageLength / 2);
            string[] spinner = { "|", "/", "-", "\\" };

            for (int i = 0; i < 50; i++)
            {
                Console.Clear();
                Console.SetCursorPosition(spaces, Console.WindowHeight / 2);
                Console.Write(message);

                Console.SetCursorPosition(spaces + message.Length + 1, Console.WindowHeight / 2);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(spinner[i % spinner.Length]);
                Console.ResetColor();

                Thread.Sleep(delay);
            }

            Console.Clear();
        }
    }
}
