using System;
using System.Linq;
using System.Threading;
using SchoolSystem.Utilities;
using SchoolSystem.Services;
using SchoolSystem.Models;

namespace SchoolSystem
{
    public class Program
    {


        public static void Main(string[] args)
        {
            //Loading.DisplayLoading("Opening System");

            // Displaying the welcome message using Wcome class
            Wcome wcome = new Wcome();
            wcome.DisplayWcome();

            Space.Spacing();

            LogIn.Login();

        }
    }
}

