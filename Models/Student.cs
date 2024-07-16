using System.Collections.Generic;

namespace SchoolSystem.Models
{
    public class Student
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public double Average { get; set; }
        public int Grade { get; set; }
        public Dictionary<string, Dictionary<string, int>> SubjectGrades { get; set; }
        public Dictionary<string, double> QuarterAverages { get; set; }
        public double FinalAverage { get; set; }

        public Student()
        {
            SubjectGrades = new Dictionary<string, Dictionary<string, int>>();
            QuarterAverages = new Dictionary<string, double>();
            FinalAverage = 0.0;
        }
    }
}
