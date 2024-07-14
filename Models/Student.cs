using System.Collections.Generic;

namespace SchoolSystem.Models
{
    public class Student
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }
        public Dictionary<string, int> SubjectGrades { get; set; }
    }
}
