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
        private Random _rnd = new Random();
        
        [Fact]
        public void AddGroup()
        {
            for (var i = 0; i < 20; i++)
            {
                var group = new Group {Name = "TG"+i, ActualCapacity = 0, MaxCapacity = _rnd.Next(5,30), TeacherId = _rnd.Next(0,50)};
                group.GroupId = Controls.InsertGroup(group);
            }

            var gId = _rnd.Next(20);
            var returnedGroup = Controls.GetGroupById(gId);
            Assert.NotNull(returnedGroup);
        }

        [Fact]
        public void AddStudent()
        {
            var rnd = new Random();
            for (var i = 0; i < 40; i++)
            {
                var st = Controls.GetStudentById(rnd.Next(1, 350));
                if (st != null)
                {
                    Controls.InsertStudentIntoGroup(st , rnd.Next(1,20));
                }
            }
            var returnedGroup = Controls.GetGroupById(18);
            
            Assert.True(returnedGroup.ActualCapacity > 0);
            }

        [Fact]
        public void AddCourse()
        {
            var rnd = new Random();
            var courses = Controls.GetCourseAll();
            for (var i = 0; i < 15; i++)
            {
                Controls.InsertCourseIntoGroup(rnd.Next(1,20), courses[rnd.Next(courses.Count)]);
            }

            var groupCourses = Controls.GetGroupCourses(1);
            Assert.True(groupCourses.Count > 0);
            
        }
        [Fact]
        public void GetAll()
        {
            var groups = Controls.GetGroupAll();
            Assert.True(groups.Count > 0);
        }

        [Fact]
        public void GetCourses()
        {
            var courses = Controls.GetGroupCourses(1);
            Assert.True(courses.Count > 0);
            
        }

        [Fact]
        public void GetById()
        {
            var group = Controls.GetGroupById(1);
            Assert.NotNull(group);
        }

        [Fact]
        public void GetByStudent()
        {
            var student = Controls.GetStudentById(111);
            var group = Controls.GetGroupByStudent(student);
            Assert.NotNull(group);
        }

        [Fact]
        public void GetByTeacher()
        {
            var teacher = Controls.GetTeacherById(3);
            var group = Controls.GetGroupByTeacher(teacher);
            Assert.NotNull(group);
        }

        [Fact]
        public void GetStudents()
        {
            var students = Controls.GetGroupUsers(1);
            Assert.True(students.Count > 0);

        }

        [Fact]
        public void Remove()
        {
            var grp = Controls.GetGroupById(1);
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
            var grp = Controls.GetGroupById(1);
            var rnd = new Random();
            var studentsStartCount = Controls.GetGroupUsers(1);
            Controls.RemoveStudentFromGroup(studentsStartCount[rnd.Next(studentsStartCount.Count)].StudentId, grp);
            var studentsEndCount = Controls.GetGroupUsers(1);
            Assert.True(studentsStartCount.Count  >  studentsEndCount.Count);
        }

        [Fact]
        public void UpdateName()
        {
            var condition = "Updated";
            Controls.UpdateGroupName(1, condition);
            var grp = Controls.GetGroupById(1);
            Assert.True(grp.Name == condition);
        }
        
        [Fact]
        public void UpdateTeacher()
        {
            var newTeacherId = 5;
            Controls.UpdateGroupTeacher(1, newTeacherId);
            var grp = Controls.GetGroupById(1);
            Assert.True(grp.TeacherId == newTeacherId );
        }

        [Fact]
        public void GetUserGroups()
        {
            var user = Controls.GetUserById(36);
            var groups = Controls.GetUserGroups(user);
            Assert.NotEmpty(groups);
        }
    }
    
    
}