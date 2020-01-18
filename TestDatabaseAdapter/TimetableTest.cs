using System;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    public class TimetableTest
    {
        private OracleDatabaseControls Controls { get; } =
            new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        [Fact]
        public void Create()
        {
            Timetables timetable = new Timetables()
                {Begin = DateTime.Parse("13:00"), End = DateTime.Parse("15:00"), ClassroomId = 1, GroupId = 1};
            timetable.TimetableId = Controls.InsertTimetable(timetable);
            Assert.True(timetable.TimetableId > 0);
        }
    }
}