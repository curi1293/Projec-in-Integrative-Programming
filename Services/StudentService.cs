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
                                    subject => int.Parse(subject.Value))
                            }).ToList();
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
                Console.WriteLine("----------------------------------------------------------------------------------------");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Subject".PadRight(20) + "1st Quarter".PadRight(15).PadLeft(17) + "2nd Quarter".PadRight(15) + "3rd Quarter".PadRight(15) + "4th Quarter".PadRight(15));
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("----------------------------------------------------------------------------------------");
                Console.ResetColor();
                
                foreach (var subjectGrade in student.SubjectGrades)
                {
                    Console.Write($"{subjectGrade.Key.PadRight(20)}: ");
                    if (subjectGrade.Value >= 75)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{subjectGrade.Value}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{subjectGrade.Value}");
                    }
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
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
