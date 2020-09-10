using System;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    public class ClassroomTest
    {
        private OracleDatabaseControls Controls { get; } = new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        [Fact]
        public void NewClassroom()
        {
            var rnd = new Random();
            for (var i = 0; i < 40; i++)
            {
                  var classroom = new Classrooms("I"+i, rnd.Next(5,30));
                            classroom.ClassroomId = Controls.InsertClassroom(classroom);
            }

            var testId = rnd.Next(0, 40);
            var testReturn = Controls.GetClassroomById(testId);
            Assert.NotNull(testReturn);
        }
    }
}