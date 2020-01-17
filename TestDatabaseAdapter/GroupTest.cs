using System;
using System.Collections.Generic;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    public class GroupTest
    {
        private OracleDatabaseControls Controls { get; } = new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        
        [Fact]
        public void AddGroup()
        {
            Group group = new Group(){Name = "TG2", ActualCapacity = 0, MaxCapacity = 20, TeacherId = 3};
            group.GroupId = Controls.InsertGroup(group);
            Group returnedGroup = Controls.GetGroupById(22);
            Assert.True(group.TeacherId == returnedGroup.TeacherId);
        }

        [Fact]
        public void AddStudent()
        {
            Random rnd = new Random();
            for (int i = 0; i < 15; i++)
            {
                Students st = Controls.GetStudentById(rnd.Next(100, 150));
                if (st != null)
                {
                    Controls.InsertStudentIntoGroup(st , 22);
                }
            }
            Group returnedGroup = Controls.GetGroupById(22);
            
            Assert.True(returnedGroup.ActualCapacity > 0);
            }

        [Fact]
        public void AddCourse()
        {
            Random rnd = new Random();
            var courses = Controls.GetCourseAll();
            for (int i = 0; i < 15; i++)
            {
                Controls.InsertCourseIntoGroup(1, courses[rnd.Next(courses.Count)]);
            }

            List<Courses> groupCourses = Controls.GetGroupCourses(1);
            Assert.True(groupCourses.Count > 0);
            
        }
        [Fact]
        public void GetAll()
        {
            List<Group> groups = Controls.GetGroupAll();
            Assert.True(groups.Count > 0);
        }

        [Fact]
        public void GetCourses()
        {
            List<Courses> courses = Controls.GetGroupCourses(1);
            Assert.True(courses.Count > 0);
            
        }

        [Fact]
        public void GetById()
        {
            Group group = Controls.GetGroupById(1);
            Assert.NotNull(group);
        }

        [Fact]
        public void GetByStudent()
        {
            Students student = Controls.GetStudentById(111);
            Group group = Controls.GetGroupByStudent(student);
            Assert.NotNull(group);
        }

        [Fact]
        public void GetByTeacher()
        {
            Teachers teacher = Controls.GetTeacherById(3);
            Group group = Controls.GetGroupByTeacher(teacher);
            Assert.NotNull(group);
        }

        [Fact]
        public void GetStudents()
        {
            List<Students> students = Controls.GetGroupUsers(1);
            Assert.True(students.Count > 0);

        }

        [Fact]
        public void Remove()
        {
            Group grp = Controls.GetGroupById(1);
            grp.TeacherId = 5;
            grp.Name = "REMOVED";
            grp.GroupId = Controls.InsertGroup(grp);
            Controls.RemoveGroup(grp.GroupId);
            Assert.Null(Controls.GetGroupById(grp.GroupId));
        }

        [Fact]
        public void RemoveCourse()
        {
            //TODO Implement Course.cs + test first   
        }

        [Fact]
        public void RemoveStudent()
        {
            Group grp = Controls.GetGroupById(1);
            Random rnd = new Random();
            var studentsStartCount = Controls.GetGroupUsers(1);
            Controls.RemoveStudentFromGroup(studentsStartCount[rnd.Next(studentsStartCount.Count)].StudentId, grp);
            var studentsEndCount = Controls.GetGroupUsers(1);
            Assert.True(studentsStartCount.Count  >  studentsEndCount.Count);
        }

        [Fact]
        public void UpdateName()
        {
            string condition = "Updated";
            Controls.UpdateGroupName(1, condition);
            Group grp = Controls.GetGroupById(1);
            Assert.True(grp.Name == condition);
        }
        
        [Fact]
        public void UpdateTeacher()
        {
            int newTeacherId = 5;
            Controls.UpdateGroupTeacher(1, newTeacherId);
            Group grp = Controls.GetGroupById(1);
            Assert.True(grp.TeacherId == newTeacherId );
        }
    }
    
    
}