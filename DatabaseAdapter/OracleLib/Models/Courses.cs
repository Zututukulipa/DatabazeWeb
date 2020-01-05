namespace DatabaseAdapter.OracleLib.Models
{
    public class Courses
    {

        public int CourseId { get; set; }


        public string FullName { get; set; }


        public string ShortName { get; set; }


        public string Description { get; set; }

        public Courses(string fullName, string shortName, string description)
        {
            FullName = fullName;
            ShortName = shortName.ToUpper();
            Description = description;
        }

        public Courses()
        {
        }
    }
}
