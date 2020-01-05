using System;

namespace DatabaseAdapter.OracleLib.Models
{
    public class Timetables
    {

        public int TimetableId { get; set; }


        public int GroupId { get; set; }


        public int ClassroomId { get; set; }


        public DateTime Begin { get; set; }


        public DateTime End { get; set; }

    }
}
