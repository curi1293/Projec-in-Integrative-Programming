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
        private const string studentsXmlFilePath = "students.xml";
        private const string teachersXmlFilePath = "teachers.xml";

        public static void Main(string[] args)
        {
            //Loading.DisplayLoading("Loading");

            // Displaying the welcome message using Wcome class
            Wcome wcome = new Wcome();
            wcome.DisplayWcome();

            Space.Spacing();

            LogIn.Login();

        }
    }
}

