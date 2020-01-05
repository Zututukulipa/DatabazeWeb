using System.Collections.Generic;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    public class TeacherTest
    {
        private OracleDatabaseControls Controls { get; } =
        new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");
        [Fact]
        public void Create()
        {
            Teachers teacher = Controls.InsertTeacher(Controls.GetUserById(468));
            Teachers teacher0 = null;
            if (Controls.IsTeacher(teacher))
                teacher0 = Controls.GetTeacherById(teacher.TeacherId);
            
            Assert.True(teacher.TeacherId == teacher0?.TeacherId);

            List<Teachers> teachers = Controls.GetTeacherAll();
            Assert.True(teachers.Count > 0);

            Teachers t = Controls.GetTeacherByUser(Controls.GetUserById(1));
            Assert.NotNull(t);
        }
    }
}