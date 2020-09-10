using System;

namespace DatabaseAdapter.OracleLib.Models
{
    public class Timetables
    {

        public int TimetableId { get; set; }

        public Group Group { get; set; }

        public Classrooms Classroom { get; set; }
        
        public DateTime Begin { get; set; }

        public DateTime End { get; set; }

    }
}
