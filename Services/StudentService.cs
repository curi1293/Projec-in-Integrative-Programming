using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class StudentService
    {
        public static List<Student> ReadStudentsFromXml(string filePath)
        {
            List<Student> students = new List<Student>();

            if (File.Exists(filePath))
            {
                XElement studentsXml = XElement.Load(filePath);
                students = (from student in studentsXml.Element("students").Elements("student")
                            select new Student
                            {
                                ID = student.Element("id").Value,
                                Name = student.Element("name").Value,
                                Grade = int.Parse(student.Element("grade").Value),
                                SubjectGrades = student.Element("subjects").Elements().ToDictionary(
                                    subject => subject.Name.ToString(),
                                    subject => new Dictionary<string, int>
                                    {
                                        { "quarter1", int.Parse(subject.Element("quarter1").Value) },
                                        { "quarter2", int.Parse(subject.Element("quarter2").Value) },
                                        { "quarter3", int.Parse(subject.Element("quarter3").Value) },
                                        { "quarter4", int.Parse(subject.Element("quarter4").Value) }
                                    }),
                                QuarterAverages = new Dictionary<string, double>(),
                                FinalAverage = 0.0
                            }).ToList();

                // Calculate quarter and final averages
                foreach (var student in students)
                {
                    // Initialize quarter averages dictionary for the student
                    student.QuarterAverages = new Dictionary<string, double>();

                    // Iterate over each quarter (quarter1, quarter2, quarter3, quarter4)
                    for (int quarter = 1; quarter <= 4; quarter++)
                    {
                        // Calculate average for current quarter across all subjects
                        double quarterAverage = students
                            .Where(s => s.ID == student.ID) // Filter by student ID
                            .SelectMany(s => s.SubjectGrades) // Flatten subject grades
                            .Where(kv => kv.Value.ContainsKey($"quarter{quarter}")) // Filter by current quarter
                            .Average(kv => kv.Value[$"quarter{quarter}"]); // Calculate average

                        // Store quarter average in the student's QuarterAverages dictionary
                        student.QuarterAverages.Add($"quarter{quarter}", quarterAverage);
                    }

                    // Calculate final average
                    student.FinalAverage = student.QuarterAverages.Values.Average();
                }
            }

            return students;
        }

        public static bool IsStudentID(string id, List<Student> students)
        {
            return students.Any(s => s.ID.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public static bool AuthenticateStudent(string id, List<Student> students)
        {
            return students.Any(s => s.ID.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public static void WelcomeStudent(string id, List<Student> students)
        {
            var student = students.FirstOrDefault(s => s.ID.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (student != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"Welcome, {student.Name}! \nGrade Level: {student.Grade}");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Subject".PadRight(20) + "1st Quarter".PadRight(15) + "2nd Quarter".PadRight(15) + "3rd Quarter".PadRight(15) + "4th Quarter".PadRight(15));
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.ResetColor();

                // Display subject grades
                foreach (var subject in student.SubjectGrades)
                {
                    Console.Write($"{subject.Key.PadRight(20)}: ");
                    foreach (var quarter in subject.Value)
                    {
                        Console.ForegroundColor = GetGradeColor(quarter.Value);
                        Console.Write($"{quarter.Value.ToString().PadRight(15)}");
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                }

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.ResetColor();

                // Display quarter averages
                DisplayAverages(student.QuarterAverages);

                // Display final average with pass/fail indication
                DisplayFinalAverage(student.FinalAverage);

                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Student not found.");
                Console.ResetColor();
            }
        }

        // Method to get color based on pass/fail
        private static ConsoleColor GetGradeColor(int grade)
        {
            return grade >= 75 ? ConsoleColor.Green : ConsoleColor.Red;
        }

        // Method to display quarter averages
        private static void DisplayAverages(Dictionary<string, double> quarterAverages)
        {
            Console.Write("Average".PadRight(20));
            foreach (var quarter in quarterAverages)
            {
                Console.ForegroundColor = GetGradeColor((int)quarter.Value);
                Console.Write($"{quarter.Value.ToString("F2").PadRight(15)}");
                Console.ResetColor();
            }
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------------");
        }

        // Method to display final average with pass/fail indication
        private static void DisplayFinalAverage(double finalAverage)
        {
            Console.Write("Final Average\t");
            Console.ForegroundColor = finalAverage >= 75 ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{finalAverage.ToString("F2").PadRight(15)}");
            Console.WriteLine(finalAverage >= 75 ? "PASSED" : "FAILED");
        }


        public static Dictionary<string, int> GetSubjectGrades(int grade)
        {
            Dictionary<string, int> subjectGrades = new Dictionary<string, int>();

            switch (grade)
            {
                case 1:
                case 2:
                    subjectGrades.Add("ReadingAndLiteracy", 0);
                    subjectGrades.Add("Language", 0);
                    subjectGrades.Add("Mathematics", 0);
                    subjectGrades.Add("Makabansa", 0);
                    subjectGrades.Add("GMRC", 0);
                    break;
                case 3:
                    subjectGrades.Add("ReadingAndLiteracy", 0);
                    subjectGrades.Add("Language", 0);
                    subjectGrades.Add("Mathematics", 0);
                    subjectGrades.Add("Makabansa", 0);
                    subjectGrades.Add("GMRC", 0);
                    subjectGrades.Add("Science", 0);
                    break;
                case 4:
                case 5:
                case 6:
                    subjectGrades.Add("Filipino", 0);
                    subjectGrades.Add("English", 0);
                    subjectGrades.Add("Mathematics", 0);
                    subjectGrades.Add("Science", 0);
                    subjectGrades.Add("AP", 0);
                    subjectGrades.Add("ESP", 0);
                    break;
                default:
                    break;
            }

            return subjectGrades;
        }
    }
}
