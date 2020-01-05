using System.Collections.Generic;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    public class ClassroomTest
    {
        private DatabaseAdapter.OracleLib.OracleDatabaseControls Controls { get; } = new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        [Fact]
        public void NewClassroom()
        {
            Classrooms classroom = new Classrooms("I808", 15);
            classroom.ClassroomId = Controls.InsertClassroom(classroom);
            var testReturn = Controls.GetClassroomById(classroom.ClassroomId);
            Assert.False(classroom.ClassroomId != testReturn.ClassroomId ||
                         classroom.Capacity != testReturn.Capacity ||
                         classroom.Name != testReturn.Name);
            
            Controls.UpdateClassroomCapacity(classroom, 20);
            Controls.UpdateClassroomName(classroom, "newClassroomName");
            Controls.RemoveClassroom(classroom);
            var classroomViaId = Controls.GetClassroomById(1);
            List<Classrooms> rooms = Controls.GetClassroomByCapacity(20);
            List<Classrooms> allrooms = Controls.GetClassroomAll();
        }
    }
}