using System;
using SchoolSystem.Services;

namespace SchoolSystem.Utilities
{
    public class LogIn
    {
        private const string studentsXmlFilePath = "students.xml";
        private const string teachersXmlFilePath = "teachers.xml";
        public static void Login()
        {


            string border = new string('*', 45);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string message = "Enter Teacher or Student ID: ";
            int consoleWidth = Console.WindowWidth;
            int spaces = (consoleWidth - message.Length) / 2;

            Console.SetCursorPosition(spaces, Console.CursorTop);
            Console.Write(message);
            string id = Console.ReadLine();

            var students = StudentService.ReadStudentsFromXml(studentsXmlFilePath);
            var teachers = TeacherService.ReadTeachersFromXml(teachersXmlFilePath);

            if (StudentService.IsStudentID(id, students))
            {
                if (StudentService.AuthenticateStudent(id, students))
                {
                    StudentService.WelcomeStudent(id, students);
                }
                else
                {
                    Console.WriteLine("Student authentication failed. Access denied.");
                }
            }
            else if (TeacherService.IsTeacherID(id, teachers))
            {

                string prompt = "Enter Teacher Password: ";
                Console.Write(new string(' ', (Console.WindowWidth - message.Length) / 2));

                Console.Write(prompt);
                string password = "";
                ConsoleKeyInfo key;

                do
                {
                    key = Console.ReadKey(true);

                    // Ignore arrow keys, spacebar, tab key, and any other unwanted keys
                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter &&
                        key.Key != ConsoleKey.LeftArrow && key.Key != ConsoleKey.RightArrow &&
                        key.Key != ConsoleKey.UpArrow && key.Key != ConsoleKey.DownArrow &&
                        key.Key != ConsoleKey.Spacebar && key.Key != ConsoleKey.Tab)
                    {
                        password += key.KeyChar;
                        Console.Write("*");
                    }
                    else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.Write("\b \b");
                    }
                } while (key.Key != ConsoleKey.Enter);

                Console.WriteLine();
                if (TeacherService.AuthenticateTeacher(id, password, teachers))
                {
                    Console.Clear();
                    TeacherService.WelcomeTeacher(id, teachers, studentsXmlFilePath);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Teacher authentication failed. Access denied.");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid ID. Please enter a valid Teacher or Student ID.");
            }
            Console.ResetColor();
        }
    }
}
