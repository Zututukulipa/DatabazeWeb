using System;
using System.Collections.Generic;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    public class GradesTest
    {
        private OracleDatabaseControls Controls { get; } =
            new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        [Fact]
        public void New()
        {
            var students = Controls.GetStudentAll();
            var teachers = Controls.GetTeacherAll();
            var courses = Controls.GetCourseAll();
            var rnd = new Random();
            for (var i = 0; i < 1000; i++)
            {
                var grade = new Grades
                {
                    Created = DateTime.Today, Description = "Nice test, especially considering its testing method",
                    Value = rnd.Next(1, 5),
                    TeacherId = teachers[rnd.Next(teachers.Count)].TeacherId,
                    StudentId = students[rnd.Next(students.Count)].StudentId,
                    CourseId = courses[rnd.Next(courses.Count)].CourseId
                };
                grade.GradeId = Controls.InsertGrade(grade);
            }

            var grades = Controls.GetGrades();
            Assert.NotEmpty(grades);
        }

        [Fact]
        public void GetAll()
        {
            var grades = Controls.GetGrades();
            Assert.NotEmpty(grades);
        }

        [Fact]
        public void GetByCourse()
        {
            var course = Controls.GetCourseById(38);
            var grades = Controls.GetGrades(course);
            Assert.NotEmpty(grades);
        }

        [Fact]
        public void GetById()
        {
            var grade = Controls.GetGrade(2);
            Assert.NotNull(grade);
        }

        [Fact]
        public void GetByStudent()
        {
            var stud = Controls.GetStudentById(145);
            var grades = Controls.GetGrades(stud);
            Assert.NotEmpty(grades);
        }

        [Fact]
        public void GetByTeacher()
        {
            var teacher = Controls.GetTeacherById(5);
            var grades = Controls.GetGrades(teacher);
            Assert.NotEmpty(grades);
        }

        [Fact]
        public void Remove()
        {
            var gradeId = 500;
            Controls.RemoveGrade(gradeId);
            var grade = Controls.GetGrade(gradeId);
            Assert.Null(grade);
        }

        [Fact]
        public void UpdateCourse()
        {
            var courseId = 140;
            var gradeId = 89;
            var grade = Controls.GetGrade(gradeId);
            Controls.UpdateGradeCourse(grade, courseId);
            var updatedGrade = Controls.GetGrade(gradeId);
            Assert.True(updatedGrade.CourseId == courseId);
            
        }

        [Fact]
        public void UpdateDescription()
        {
        var description = "new updated description";
        var gradeId = 89;
        var grade = Controls.GetGrade(gradeId);
        Controls.UpdateGradeDescription(grade, description);
        var updatedGrade = Controls.GetGrade(gradeId);
        Assert.True(updatedGrade.Description == description);
        }

        [Fact]
        public void UpdateStudent()
        {
            var studentId = 81;
            var gradeId = 89;
            var grade = Controls.GetGrade(gradeId);
            Controls.UpdateGradeStudent(grade, studentId);
            var updatedGrade = Controls.GetGrade(gradeId);
            Assert.True(updatedGrade.StudentId == studentId);   
        }

        [Fact]
        public void UpdateTeacher()
        {
            var teacherId = 5;
            var gradeId = 89;
            var grade = Controls.GetGrade(gradeId);
            Controls.UpdateGradeTeacher(grade, teacherId);
            var updatedGrade = Controls.GetGrade(gradeId);
            Assert.True(updatedGrade.TeacherId == teacherId);   
        }
        
        [Fact]
        public void UpdateValue()
        {
            var rnd = new Random();
            var value = rnd.Next(1,5);
            var gradeId = 89;
            var grade = Controls.GetGrade(gradeId);
            Controls.UpdateGradeValue(grade, value);
            var updatedGrade = Controls.GetGrade(gradeId);
            Assert.True(updatedGrade.Value == value);   
        }
    }
}