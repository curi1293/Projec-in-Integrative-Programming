using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using SchoolSystem.Models;
using SchoolSystem.Utilities;

namespace SchoolSystem.Services
{
    public class TeacherService
    {
        public static List<Teacher> ReadTeachersFromXml(string filePath)
        {
            List<Teacher> teachers = new List<Teacher>();

            if (File.Exists(filePath))
            {
                XElement teachersXml = XElement.Load(filePath);
                teachers = (from teacher in teachersXml.Element("teachers").Elements("teacher")
                            select new Teacher
                            {
                                ID = teacher.Element("id").Value,
                                Password = teacher.Element("password").Value,
                                GradeLevel = int.Parse(teacher.Element("gradeLevel").Value)
                            }).ToList();
            }

            return teachers;
        }

        public static bool IsTeacherID(string id, List<Teacher> teachers)
        {
            return teachers.Any(t => t.ID.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public static bool AuthenticateTeacher(string id, string password, List<Teacher> teachers)
        {
            return teachers.Any(t => t.ID.Equals(id, StringComparison.OrdinalIgnoreCase) && t.Password.Equals(password));
        }

        public static void WelcomeTeacher(string id, List<Teacher> teachers, string studentsXmlFilePath)
        {
            var teacher = teachers.FirstOrDefault(t => t.ID.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (teacher != null)
            {
                bool exit = false;
                while (!exit)
                {
                    DisplayWelcomeMessage(teacher);
                    DisplayStudentListHeader();
                    DisplayStudentsByGradeLevel(teacher.GradeLevel, studentsXmlFilePath);
                    HandleTeacherOptions(id, teacher, studentsXmlFilePath, ref exit);
                }
            }
            else
            {
                Console.WriteLine("Teacher not found.");
            }
        }

        private static void DisplayWelcomeMessage(Teacher teacher)
        {
            Console.Clear();
            SchoolHeader schoolHeader = new SchoolHeader();
            schoolHeader.DisplayHeader();

            Space.Spacing();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"\t\tWelcome, Teacher with ID: {teacher.ID}!");
            Console.WriteLine($"\t\tGrade Level: {teacher.GradeLevel}");
            Console.ResetColor();
            Console.WriteLine();
        }

        //public class Average
        //{
        //public static string[] quarters = { "1st Quarter", "2nd Quarter", "3rd Quarter", "4th Quarter", "Final" };
        //}
        private static void DisplayStudentListHeader()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string listOfStudents = "LIST OF STUDENTS";
            Console.WriteLine(listOfStudents.PadLeft((Console.WindowWidth + listOfStudents.Length) / 2));
            Console.ResetColor();

            //string[] Quarter = Average.quarters;
            string header = "ID".PadRight(15) + "Name" + "\t\t\t1st Quarter" + "\t2nd Quarter" + "\t3rd Quarter" + "\t4th Quarter" + "\tFinal";

            //foreach (var quarter in Quarter)
            //{header += "\t\t\t" + quarter;}

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\t" + header);
            Console.ResetColor();
        }

        private static void DisplayStudentsByGradeLevel(int gradeLevel, string studentsXmlFilePath)
        {
            if (File.Exists(studentsXmlFilePath))
            {
                XElement studentsXml = XElement.Load(studentsXmlFilePath);
                var students = from student in studentsXml.Element("students").Elements("student")
                               where int.Parse(student.Element("grade").Value) == gradeLevel
                               select new
                               {
                                   ID = student.Element("id").Value,
                                   Name = student.Element("name").Value,
                                   Grades = new
                                   {
                                       Quarter1 = student.Element("subjects")?.Element("quarter1") != null ? int.Parse(student.Element("subjects").Element("quarter1").Value) : 0,
                                       Quarter2 = student.Element("subjects")?.Element("quarter2") != null ? int.Parse(student.Element("subjects").Element("quarter2").Value) : 0,
                                       Quarter3 = student.Element("subjects")?.Element("quarter3") != null ? int.Parse(student.Element("subjects").Element("quarter3").Value) : 0,
                                       Quarter4 = student.Element("subjects")?.Element("quarter4") != null ? int.Parse(student.Element("subjects").Element("quarter4").Value) : 0
                                   }
                               };

                foreach (var student in students)
                {
                    // Calculate quarter averages
                    double quarter1Avg = student.Grades.Quarter1;
                    double quarter2Avg = student.Grades.Quarter2;
                    double quarter3Avg = student.Grades.Quarter3;
                    double quarter4Avg = student.Grades.Quarter4;

                    // Calculate overall average
                    double overallAvg = (quarter1Avg + quarter2Avg + quarter3Avg + quarter4Avg) / 4.0;

                    Console.WriteLine($"\t{student.ID}".PadRight(15) + $" {student.Name}" +
                                      $"\t\t\t{student.Grades.Quarter1}\t{student.Grades.Quarter2}\t{student.Grades.Quarter3}\t{student.Grades.Quarter4}" +
                                      $"\t\t\tQuarter Avg: {overallAvg:F2}");

                    // Displaying only the average (not the grade)
                    Console.WriteLine($"\t\t\tAverage: {overallAvg:F2}");
                }

            }
            else
            {
                Console.WriteLine("Student file not found.");
            }
        }

        private static void HandleTeacherOptions(string id, Teacher teacher, string studentsXmlFilePath, ref bool exit)
        {
            string[] options = { "Display Student List", "Add Student", "Edit Student", "Delete Student", "Exit" };
            int currentSelection = 0;

            while (!exit)
            {
                Console.Clear();
                DisplayWelcomeHeader(); // Display header before options
                DisplayWelcomeMessage(teacher); // Pass teacher object to display the welcome message with ID and name

                Console.WriteLine();

                // Calculate the width of the longest option for proper centering and border
                int maxLength = options.Max(option => option.Length) + 4;
                int totalWidth = maxLength + 4; // Adding 4 for border spaces

                // Center the options block
                int leftPadding = (Console.WindowWidth - totalWidth) / 2;

                // Display top border
                Console.WriteLine(new string(' ', leftPadding) + new string('=', totalWidth));

                for (int i = 0; i < options.Length; i++)
                {
                    string optionLine = $"| {options[i].PadRight(maxLength)} |";
                    if (i == currentSelection)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(new string(' ', leftPadding) + optionLine);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(new string(' ', leftPadding) + optionLine);
                    }
                }

                // Display bottom border
                Console.WriteLine(new string(' ', leftPadding) + new string('=', totalWidth));

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentSelection > 0)
                        {
                            currentSelection--;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (currentSelection < options.Length - 1)
                        {
                            currentSelection++;
                        }
                        break;

                    case ConsoleKey.Enter:
                        switch (currentSelection)
                        {
                            case 0:
                                Console.Clear();
                                DisplayWelcomeHeader();
                                DisplayStudentListHeader();
                                DisplayStudentsByGradeLevel(teacher.GradeLevel, studentsXmlFilePath);
                                GradingOption(id, teacher, studentsXmlFilePath, ref exit);
                                PauseAndReturnToMain();
                                break;
                            case 1:
                                Console.Clear();
                                DisplayWelcomeHeader();
                                AddStudent(teacher.GradeLevel, studentsXmlFilePath);
                                PauseAndReturnToMain();
                                break;
                            case 2:
                                Console.Clear();
                                DisplayWelcomeHeader();
                                EditStudent(studentsXmlFilePath, teacher.GradeLevel);
                                PauseAndReturnToMain();
                                break;
                            case 3:
                                Console.Clear();
                                DisplayWelcomeHeader();
                                DeleteStudent(studentsXmlFilePath);
                                PauseAndReturnToMain();
                                break;
                            case 4:
                                exit = true;
                                Console.Clear();
                                Wcome wcome = new Wcome();
                                wcome.DisplayWcome();
                                Space.Spacing();
                                LogIn.Login();

                                break;
                        }
                        break;
                }
            }
        }
        private const string teachersXmlFilePath = "teachers.xml";
        private static void GradingOption(string id, Teacher teacher, string studentsXmlFilePath, ref bool exit)
        {
            string[] options = { "Add Grade", "Edit Grade", "Back" };
            int currentSelection = 0;

            while (!exit)
            {
                Console.Clear();
                DisplayWelcomeHeader();
                DisplayStudentListHeader();
                DisplayStudentsByGradeLevel(teacher.GradeLevel, studentsXmlFilePath);


                Console.WriteLine();

                int maxLength = options.Max(option => option.Length) + 4;
                int totalWidth = maxLength + 4;

                int leftPadding = (Console.WindowWidth - totalWidth) / 2;

                Console.WriteLine(new string(' ', leftPadding) + new string('=', totalWidth));
                var teachers = TeacherService.ReadTeachersFromXml(teachersXmlFilePath);
                for (int i = 0; i < options.Length; i++)
                {
                    string optionLine = $"| {options[i].PadRight(maxLength)} |";
                    if (i == currentSelection)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(new string(' ', leftPadding) + optionLine);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(new string(' ', leftPadding) + optionLine);
                    }
                }

                Console.WriteLine(new string(' ', leftPadding) + new string('=', totalWidth));

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentSelection > 0)
                        {
                            currentSelection--;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (currentSelection < options.Length - 1)
                        {
                            currentSelection++;
                        }
                        break;

                    case ConsoleKey.Enter:
                        switch (currentSelection)
                        {
                            case 0:
                                Console.Clear();
                                DisplayWelcomeHeader();
                                AddGrade(studentsXmlFilePath);
                                PauseAndReturnToMain();
                                break;
                            case 1:
                                Console.Clear();
                                DisplayWelcomeHeader();
                                EditGrade(studentsXmlFilePath);
                                PauseAndReturnToMain();
                                break;
                            case 2:
                                exit = true;
                                Console.Clear();
                                WelcomeTeacher(id, teachers, studentsXmlFilePath);
                                break;
                        }
                        break;
                }
            }
        }

        private static void AddGrade(string studentsXmlFilePath)
        {
            XElement studentsXml = XElement.Load(studentsXmlFilePath);
            XElement studentsElement = studentsXml.Element("students");

            Console.Write("Enter Student ID to Add Grade: ");
            string id = Console.ReadLine();

            XElement studentElement = studentsElement.Elements("student")
                .FirstOrDefault(e => e.Element("id").Value.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (studentElement != null)
            {
                Console.WriteLine($"Adding grades for Student ID: {id}");

                // Display subjects and current grades
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("\nSubject".PadRight(20) + "1st Quarter".PadRight(15) + "2nd Quarter".PadRight(15) + "3rd Quarter".PadRight(15) + "4th Quarter".PadRight(15));
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.ResetColor();

                foreach (var subjectElement in studentElement.Element("subjects").Elements())
                {
                    Console.Write($"{subjectElement.Name.ToString().PadRight(20)}: ");
                    foreach (var quarterElement in subjectElement.Elements())
                    {
                        int grade = int.Parse(quarterElement.Value);
                        Console.ForegroundColor = GetGradeColor(grade);
                        Console.Write($"{grade.ToString().PadRight(15)}");
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("\nQuarter to add subject:");
                Console.WriteLine("Enter Grades per Subject:");

                // Update grades
                foreach (var subjectElement in studentElement.Element("subjects").Elements())
                {
                    Console.Write($"{subjectElement.Name}: ");
                    if (int.TryParse(Console.ReadLine(), out int newGrade) && newGrade >= 0)
                    {
                        subjectElement.Value = newGrade.ToString();
                    }
                }

                // Save changes to XML file
                studentsXml.Save(studentsXmlFilePath);
                Console.WriteLine("Grades added successfully.");
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }

        private static ConsoleColor GetGradeColor(int grade)
        {
            return grade >= 75 ? ConsoleColor.Green : ConsoleColor.Red;
        }
        private static void EditGrade(string studentsXmlFilePath)
        {
            XElement studentsXml = XElement.Load(studentsXmlFilePath);
            XElement studentsElement = studentsXml.Element("students");

            Console.Write("Enter Student ID to edit grade: ");
            string id = Console.ReadLine();

            XElement studentElement = studentsElement.Elements("student")
                .FirstOrDefault(e => e.Element("id").Value.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (studentElement != null)
            {
                Console.WriteLine($"Editing grades for Student ID: {id}");

                // Display subjects and current grades
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("\nSubject".PadRight(20) + "1st Quarter".PadRight(15) + "2nd Quarter".PadRight(15) + "3rd Quarter".PadRight(15) + "4th Quarter".PadRight(15));
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.ResetColor();

                foreach (var subjectElement in studentElement.Element("subjects").Elements())
                {
                    Console.Write($"{subjectElement.Name.ToString().PadRight(20)}: ");
                    foreach (var quarterElement in subjectElement.Elements())
                    {
                        int grade = int.Parse(quarterElement.Value);
                        Console.ForegroundColor = GetGradeColor(grade);
                        Console.Write($"{grade.ToString().PadRight(15)}");
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                }

                // Update grades
                Console.WriteLine("\nQuarter to edit subject grade:");
                Console.WriteLine("\nEnter new grades:");
                foreach (var subjectElement in studentElement.Element("subjects").Elements())
                {
                    Console.Write($"{subjectElement.Name}: ");
                    if (int.TryParse(Console.ReadLine(), out int newGrade) && newGrade >= 0)
                    {
                        subjectElement.Element("quarter1").Value = newGrade.ToString();
                        subjectElement.Element("quarter2").Value = newGrade.ToString();
                        subjectElement.Element("quarter3").Value = newGrade.ToString();
                        subjectElement.Element("quarter4").Value = newGrade.ToString();
                    }
                }

                // Save changes to XML file
                studentsXml.Save(studentsXmlFilePath);
                Console.WriteLine("Grades updated successfully.");
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }



        private static void DisplayWelcomeHeader()
        {
            SchoolHeader schoolHeader = new SchoolHeader();
            schoolHeader.DisplayHeader();
            Space.Spacing(); // Assuming this is a method or class for spacing
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\t\tTeacher Options");
            Console.ResetColor();
        }

        private static void AddStudent(int gradeLevel, string studentsXmlFilePath)
        {
            XElement studentsXml = XElement.Load(studentsXmlFilePath);
            XElement studentsElement = studentsXml.Element("students");

            Console.Write("Enter Student ID: ");
            string id = Console.ReadLine();
            Console.Write("Enter Student Name: ");
            string name = Console.ReadLine();

            XElement newStudent = new XElement("student",
                new XElement("id", id),
                new XElement("name", name),
                new XElement("grade", gradeLevel),
                new XElement("subjects")
            );

            Dictionary<string, int> subjectGrades = GetSubjectGrades(gradeLevel);
            foreach (var subject in subjectGrades)
            {
                newStudent.Element("subjects").Add(new XElement(subject.Key, subject.Value));
            }

            studentsElement.Add(newStudent);
            studentsXml.Save(studentsXmlFilePath);
            Console.WriteLine("Student added successfully.");
        }

        private static void EditStudent(string studentsXmlFilePath, int teacherGradeLevel)
        {
            XElement studentsXml = XElement.Load(studentsXmlFilePath);
            XElement studentsElement = studentsXml.Element("students");

            Console.Write("Enter Student ID to edit: ");
            string id = Console.ReadLine();

            XElement studentElement = studentsElement.Elements("student")
                .FirstOrDefault(e => e.Element("id").Value.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (studentElement != null)
            {
                Student studentToEdit = new Student
                {
                    ID = studentElement.Element("id").Value,
                    Name = studentElement.Element("name").Value,
                    Grade = int.Parse(studentElement.Element("grade").Value),
                    SubjectGrades = studentElement.Element("subjects").Elements()
                        .ToDictionary(e => e.Name.LocalName, e => new Dictionary<string, int>
                        {
                    { "quarter1", int.Parse(e.Element("quarter1").Value) },
                    { "quarter2", int.Parse(e.Element("quarter2").Value) },
                    { "quarter3", int.Parse(e.Element("quarter3").Value) },
                    { "quarter4", int.Parse(e.Element("quarter4").Value) }
                        })
                };

                if (!IsTeacherAuthorized(teacherGradeLevel, studentToEdit.Grade))
                {
                    Console.WriteLine($"Teacher is not authorized to manage students in grade {studentToEdit.Grade}.");
                    return;
                }

                Console.WriteLine("Choose what to edit:");
                Console.WriteLine("1. Name");
                Console.WriteLine("2. Student ID");
                Console.WriteLine("3. Grades per Subject");
                Console.Write("Enter your choice: ");
                string editChoice = Console.ReadLine();
                switch (editChoice)
                {
                    case "1":
                        Console.Write("Enter New Student Name: ");
                        string newStudentName = Console.ReadLine();
                        studentToEdit.Name = newStudentName;
                        studentElement.Element("name").Value = newStudentName;
                        break;
                    case "2":
                        Console.Write("Enter New Student ID: ");
                        string newStudentID = Console.ReadLine();
                        studentToEdit.ID = newStudentID;
                        studentElement.Element("id").Value = newStudentID;
                        break;
                    case "3":
                        Console.WriteLine("Enter Grades per Subject:");

                        foreach (var subjectElement in studentElement.Element("subjects").Elements())
                        {
                            Console.WriteLine($"{subjectElement.Name}:");
                            foreach (var quarterElement in subjectElement.Elements())
                            {
                                Console.Write($"{quarterElement.Name}: ");
                                if (int.TryParse(Console.ReadLine(), out int newGrade) && newGrade >= 0)
                                {
                                    quarterElement.Value = newGrade.ToString();
                                }
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        return;
                }

                studentsXml.Save(studentsXmlFilePath);
                Console.WriteLine("Student data edited successfully.");
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }

        private static bool IsTeacherAuthorized(int teacherGradeLevel, int studentGradeLevel)
        {
            return teacherGradeLevel == studentGradeLevel;
        }

        private static Dictionary<string, int> GetSubjectGrades(int gradeLevel)
        {
            Dictionary<string, int> subjectGrades = new Dictionary<string, int>();

            switch (gradeLevel)
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
                    subjectGrades.Add("English", 0);
                    subjectGrades.Add("Filipino", 0);
                    subjectGrades.Add("Mathematics", 0);
                    subjectGrades.Add("AralingPanlipunan", 0);
                    subjectGrades.Add("Science", 0);
                    subjectGrades.Add("MAPEH", 0);
                    subjectGrades.Add("EPP", 0);
                    break;
                default:
                    break;
            }

            return subjectGrades;
        }

        private static void DeleteStudent(string studentsXmlFilePath)
        {
            XElement studentsXml = XElement.Load(studentsXmlFilePath);
            XElement studentsElement = studentsXml.Element("students");

            Console.Write("Enter Student ID to delete: ");
            string id = Console.ReadLine();

            XElement studentElement = studentsElement.Elements("student")
                .FirstOrDefault(e => e.Element("id").Value.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (studentElement != null)
            {
                Console.Write("Are you sure you want to delete this student? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    studentElement.Remove();
                    studentsXml.Save(studentsXmlFilePath);
                    Console.WriteLine("Student deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Delete operation cancelled.");
                }
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }

        private static void PauseAndReturnToMain()
        {
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
