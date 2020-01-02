using System.Collections.Generic;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    public class ClassroomTest
    {
        private List<Classrooms> Classrooms1 { get; } = new List<Classrooms>();
        private DatabaseAdapter.OracleLib.OracleDatabaseControls Controls { get; } = new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        [Fact]
        public void NewClassroom()
        {
            Classrooms classroom = new Classrooms("I808", 15);
            classroom.ClassroomId = Controls.InsertClassroom(classroom);
        }
    }
}