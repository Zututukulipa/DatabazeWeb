namespace DatabaseAdapter.OracleLib.Models
{
    public class Group
    {

        public int GroupId { get; set; }


        public int TeacherId { get; set; }


        public string Name { get; set; }


        public int MaxCapacity { get; set; }


        public int ActualCapacity { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public Group()
        {
            GroupId = 666;
            TeacherId = 0;
            Name = "Mockup";
            MaxCapacity = 20;
            ActualCapacity = 0;
        }
    }
}
