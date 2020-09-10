using System;

namespace DatabaseAdapter.OracleLib.Models
{
    public class Grades
    {

        public int GradeId { get; set; }

        public DateTime Created { get; set; }


        public int StudentId { get; set; }


        public int TeacherId { get; set; }


        public int CourseId { get; set; }


        public int Value { get; set; }


        public string Description { get; set; }
    }
}
