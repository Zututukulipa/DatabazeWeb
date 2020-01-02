using System;

namespace DatabaseAdapter.OracleLib.Models
{
    public class Timetables
    {

        public string TimetableId { get; set; }


        public string GroupId { get; set; }


        public string ClassroomId { get; set; }


        public DateTime Begin { get; set; }


        public DateTime End { get; set; }

    }
}
