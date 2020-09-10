using System;
using System.Collections.Generic;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    public class StudentTest
    {
        private OracleDatabaseControls Controls { get; } =
            new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        [Fact]
        public void Create()
        {
            var rnd = new Random();
            for (var i = 0; i < 20; i++)
            {
                var us = Controls.GetUserById(rnd.Next(50, 500));
                if (us != null)
                {
                    Controls.InsertStudent(us);
                }
            }
            var student = Controls.GetStudentById(Controls.InsertStudent(Controls.GetUserById(rnd.Next(50,500))));
            var students = Controls.GetStudentAll();
            Assert.NotNull(students.Find(std => student.StudentId == std.StudentId));
        }

        [Fact]
        public void Get()
        {
            var student = Controls.GetStudentById(21);
            Assert.True(student.StudentId == 21);
        }
[Fact]
        public void Update()
        {
            var us = Controls.GetUserById(666);
            Assert.True(Controls.IsStudent(us));
            var student = Controls.GetStudentById(21);
            Controls.UpdateStudentYear(student, 2);
            var updatedStudent = Controls.GetStudentById(21);
            Assert.True(updatedStudent.Year == 2);
            
        }
    }
}