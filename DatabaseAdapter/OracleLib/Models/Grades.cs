using System;

namespace DatabaseAdapter.OracleLib.Models
{
    public class Grades
    {

        public string GradeId { get; set; }


        public string StudentId { get; set; }


        public string TeacherId { get; set; }


        public string CourseId { get; set; }


        public string Value { get; set; }


        public string Description { get; set; }


        public DateTime Created { get; set; }

    }
}
