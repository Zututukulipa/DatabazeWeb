using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Enums;
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
            Random rnd = new Random();
            for (int i = 0; i < 20; i++)
            {
                User us = Controls.GetUserById(rnd.Next(50, 500));
                if (us != null)
                {
                    Controls.InsertStudent(us);
                }
            }
            Students student = Controls.GetStudentById(Controls.InsertStudent(Controls.GetUserById(rnd.Next(50,500))));
            List<Students> students = Controls.GetStudentAll();
            Assert.NotNull(students.Find(std => student.StudentId == std.StudentId));
        }

        [Fact]
        public void Get()
        {
            Students student = Controls.GetStudentById(21);
            Assert.True(student.StudentId == 21);
        }
[Fact]
        public void Update()
        {
            User us = Controls.GetUserById(666);
            Assert.True(Controls.IsStudent(us));
            Students student = Controls.GetStudentById(21);
            Controls.UpdateStudentYear(student, 2);
            Students updatedStudent = Controls.GetStudentById(21);
            Assert.True(updatedStudent.Year == 2);
            
        }
    }
}