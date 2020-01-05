using System.Collections.Generic;
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
            User user = Controls.GetUserById(777);
            Students student = Controls.GetStudentById(Controls.InsertStudent(user));
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