using System;

namespace SchoolSystem.Utilities
{
    public class Wcome
    {
        public void DisplayWcome()
        {
            int windowWidth = Console.WindowWidth;

            string welcomeText = @"
                ____    _____   _        _    _    _____               ____                    
               |  _ \  |_   _| | |      | |  | |  / ____|     /\      / __ \                   
               | |_) |   | |   | |      | |  | | | |         /  \    | |  | |                  
               |  _ <    | |   | |      | |  | | | |        / /\ \   | |  | |                  
               | |_) |  _| |_  | |____  | |__| | | |____   / ____ \  | |__| |                  
               |____/  |_____| |______|  \____/   \_____| /_/    \_\  \____/                   
  ______   _        ______   __  __   ______   _   _   _______              _____   __     __  
 |  ____| | |      |  ____| |  \/  | |  ____| | \ | | |__   __|     /\     |  __ \  \ \   / /  
 | |__    | |      | |__    | \  / | | |__    |  \| |    | |       /  \    | |__) |  \ \_/ /   
 |  __|   | |      |  __|   | |\/| | |  __|   | . ` |    | |      / /\ \   |  _  /    \   /    
 | |____  | |____  | |____  | |  | | | |____  | |\  |    | |     / ____ \  | | \ \     | |     
 |______| |______| |______| |_|  |_| |______| |_| \_|    |_|    /_/    \_\ |_|  \_\    |_|     
                     _____    _____   _    _    ____     ____    _                             
                    / ____|  / ____| | |  | |  / __ \   / __ \  | |                            
                   | (___   | |      | |__| | | |  | | | |  | | | |                            
                    \___ \  | |      |  __  | | |  | | | |  | | | |                            
                    ____) | | |____  | |  | | | |__| | | |__| | | |____                        
                   |_____/   \_____| |_|  |_|  \____/   \____/  |______|                       
            ";

            string[] lines = welcomeText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();

            foreach (var line in lines)
            {
                Console.WriteLine(line.PadLeft((windowWidth + line.Length) / 2));
            }

            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
