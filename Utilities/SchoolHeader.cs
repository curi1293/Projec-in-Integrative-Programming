using System;

namespace SchoolSystem.Utilities
{
    public class SchoolHeader
    {
        public void DisplayHeader()
        {
            int windowWidth = Console.WindowWidth;

            string line1 = "Republic of the Philippines";
            string line2 = "Department of Education";
            string line3 = "Region IV-A CALABARZON";
            string line4 = "Bilucao Elementary School";

            // Set the text and background colors for the header
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGreen;

            // Helper method to print centered text with full-width background
            void PrintCentered(string text)
            {
                string paddedText = text.PadLeft((windowWidth + text.Length) / 2).PadRight(windowWidth);
                Console.WriteLine(paddedText);
            }

            // Print the border
            string border = new string('*', windowWidth);
            Console.WriteLine(border);

            // Print the header with dark green background
            PrintCentered(line1);
            PrintCentered(line2);
            PrintCentered(line3);
            PrintCentered(line4);
            Console.WriteLine(border); // Print the bottom border

            // Reset the colors to their defaults for the rest of the console
            Console.ResetColor();
        }
    }
}
