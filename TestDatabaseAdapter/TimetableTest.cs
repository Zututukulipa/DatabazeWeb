using System;
using System.Collections.Generic;
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
            var rnd = new Random();
            var groups = Controls.GetGroupAll();
            foreach (var group in groups)
            {
                for (var i = 0; i < 6; i++)
                {
                    var dat = RandomDay();
                    var timetable = new Timetables
                    {
                        Begin = dat, End = dat.AddHours(rnd.Next(8,20)),
                        Classroom = new Classrooms{ClassroomId = rnd.Next(1, 40)}, Group = new Group(){GroupId = group.GroupId}
                    };
                    timetable.TimetableId = Controls.InsertTimetable(timetable);
                }
            }
        }

        DateTime RandomDay()
        {
            var gen = new Random();

            var start = new DateTime(2020, 1, 1);
            var range = 6;
            return start.AddDays(gen.Next(range));
        }
    }
}