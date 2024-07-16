using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using SchoolSystem.Models;
using SchoolSystem.Utilities;

namespace SchoolSystem.Services
{
    public class GradingsStudents
    {
        private static void HandleStudentSelection(List<dynamic> students, string studentsXmlFilePath, int gradeLevel)
        {
            int currentSelection = 0;

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentSelection > 0)
                        {
                            currentSelection--;
                            DisplayStudentListWithSelection(students, currentSelection);
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (currentSelection < students.Count - 1)
                        {
                            currentSelection++;
                            DisplayStudentListWithSelection(students, currentSelection);
                        }
                        break;

                    case ConsoleKey.Enter:
                        DisplayStudentActions(students[currentSelection].ID, studentsXmlFilePath, gradeLevel);
                        return;
                }
            }
        }

        private static void DisplayStudentListWithSelection(List<dynamic> students, int selectedIndex)
        {
            Console.Clear();

            for (int i = 0; i < students.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\t> {students[i].ID}".PadRight(20) + $"{students[i].Name}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"\t  {students[i].ID}".PadRight(20) + $"{students[i].Name}");
                }
            }

            Console.WriteLine("\nUse the arrow keys to navigate, Enter to select a student.");
        }

        private static void DisplayStudentActions(string studentID, string studentsXmlFilePath, int gradeLevel)
        {
            string[] actions = { "Add Grade", "Edit Grade", "Back" };
            int currentAction = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Selected Student ID: {studentID}\n");

                for (int i = 0; i < actions.Length; i++)
                {
                    if (i == currentAction)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\t> {actions[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"\t  {actions[i]}");
                    }
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentAction > 0)
                        {
                            currentAction--;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (currentAction < actions.Length - 1)
                        {
                            currentAction++;
                        }
                        break;

                    case ConsoleKey.Enter:
                        switch (currentAction)
                        {
                            case 0:
                                AddGrade(studentID, studentsXmlFilePath, gradeLevel);
                                return;
                            case 1:
                                EditGrade(studentID, studentsXmlFilePath, gradeLevel);
                                return;
                            case 2:
                                return;
                        }
                        break;
                }
            }
        }

        private static void AddGrade(string studentID, string studentsXmlFilePath, int gradeLevel)
        {
            XElement studentsXml = XElement.Load(studentsXmlFilePath);
            XElement studentElement = studentsXml.Element("students").Elements("student")
                .FirstOrDefault(e => e.Element("id").Value.Equals(studentID, StringComparison.OrdinalIgnoreCase));

            if (studentElement != null)
            {
                Console.WriteLine("Enter Grades for the following subjects:");
                foreach (var subjectElement in studentElement.Element("subjects").Elements())
                {
                    Console.Write($"{subjectElement.Name}: ");
                    if (int.TryParse(Console.ReadLine(), out int newGrade) && newGrade >= 0)
                    {
                        subjectElement.Value = newGrade.ToString();
                    }
                }

                studentsXml.Save(studentsXmlFilePath);
                Console.WriteLine("Grades added successfully.");
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }

        private static void EditGrade(string studentID, string studentsXmlFilePath, int gradeLevel)
        {
            XElement studentsXml = XElement.Load(studentsXmlFilePath);
            XElement studentElement = studentsXml.Element("students").Elements("student")
                .FirstOrDefault(e => e.Element("id").Value.Equals(studentID, StringComparison.OrdinalIgnoreCase));

            if (studentElement != null)
            {
                Console.WriteLine("Enter new grades for the following subjects:");
                foreach (var subjectElement in studentElement.Element("subjects").Elements())
                {
                    Console.Write($"{subjectElement.Name}: ");
                    if (int.TryParse(Console.ReadLine(), out int newGrade) && newGrade >= 0)
                    {
                        subjectElement.Value = newGrade.ToString();
                    }
                }

                studentsXml.Save(studentsXmlFilePath);
                Console.WriteLine("Grades edited successfully.");
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }

    }
}