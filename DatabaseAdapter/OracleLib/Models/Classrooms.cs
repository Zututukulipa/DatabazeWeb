namespace DatabaseAdapter.OracleLib.Models
{
    public class Classrooms
    {

        public int ClassroomId { get; set; }


        public string Name { get; set; }


        public int Capacity { get; set; }

        public Classrooms()
        {
        }

        public Classrooms(string name, int capacity)
        {
            Name = name;
            Capacity = capacity;
        }
    }
}
