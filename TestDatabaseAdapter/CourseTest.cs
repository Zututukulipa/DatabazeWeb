using System.Collections.Generic;
using System.IO;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Microsoft.VisualBasic.FileIO;
using Xunit;

namespace TestDatabaseAdapter
{
    public class GroupsIntegrationTest
    {
        private readonly OracleDatabaseControls _controls =
            new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        [Fact]
        public void CoursesIntegration()
        {
            var courses = LoadCoursesFromCsv("/home/zututukulipa/RiderProjects/Databaze/TestDatabaseAdapter/searchSimplePredmety-2019-12-16-16-05.csv");
            foreach (var course in courses)
            {
                course.CourseId = _controls.InsertCourse(course);
            }
            
            _controls.RemoveCourse(courses[57]);
            courses[0].FullName = "New Updated FullName";
            courses[0].Description = "New [UPDATED] Description";
            courses[0].ShortName = "NUFSH";
            _controls.UpdateCourseFullName(courses[0]);
            _controls.UpdateCourseShortcut(courses[0]);
            _controls.UpdateCourseDescription(courses[0]);
            
            List<Courses> allCourses = _controls.GetCourseAll();
            Courses crs = _controls.GetCourseById(courses[0].CourseId);
            Assert.True(crs.Description == "New [UPDATED] Description" && crs.FullName ==  "New Updated FullName" && crs.ShortName == "NUFSH" && allCourses.Count > 0);

        }
        
        protected List<Courses> LoadCoursesFromCsv(string path)
        {
            List<Courses> courses = new List<Courses>();
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                parser.ReadFields();
                while (!parser.EndOfData) 
                {
                    //Processing row
                    string[] fields = parser.ReadFields();
                    courses.Add(parseCourses(fields));
                }
            }

            return courses;
        }

        private Courses parseCourses(string[] fields)
        {
            if (fields[29].Length > 250)
            {
                fields[29] = fields[29].Substring(0,240) + "...";
            }
            return new Courses(fields[4],fields[2], fields[29]);
        }
    }
}