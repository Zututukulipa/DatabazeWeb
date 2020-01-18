using System;
using System.Collections.Generic;
using System.Net;
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
            List<Students> students = Controls.GetStudentAll();
            List<Teachers> teachers = Controls.GetTeacherAll();
            List<Courses> courses = Controls.GetCourseAll();
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                Grades grade = new Grades()
                {
                    Created = DateTime.Today, Description = "Nice test, especially considering its testing method",
                    Value = rnd.Next(1, 5),
                    TeacherId = teachers[rnd.Next(teachers.Count)].TeacherId,
                    StudentId = students[rnd.Next(students.Count)].StudentId,
                    CourseId = courses[rnd.Next(courses.Count)].CourseId
                };
                grade.GradeId = Controls.InsertGrade(grade);
            }

            List<Grades> grades = Controls.GetGrades();
            Assert.NotEmpty(grades);
        }

        [Fact]
        public void GetAll()
        {
            List<Grades> grades = Controls.GetGrades();
            Assert.NotEmpty(grades);
        }

        [Fact]
        public void GetByCourse()
        {
            Courses course = Controls.GetCourseById(38);
            List<Grades> grades = Controls.GetGrades(course);
            Assert.NotEmpty(grades);
        }

        [Fact]
        public void GetById()
        {
            Grades grade = Controls.GetGrade(2);
            Assert.NotNull(grade);
        }

        [Fact]
        public void GetByStudent()
        {
            Students stud = Controls.GetStudentById(145);
            List<Grades> grades = Controls.GetGrades(stud);
            Assert.NotEmpty(grades);
        }

        [Fact]
        public void GetByTeacher()
        {
            Teachers teacher = Controls.GetTeacherById(5);
            List<Grades> grades = Controls.GetGrades(teacher);
            Assert.NotEmpty(grades);
        }

        [Fact]
        public void Remove()
        {
            int gradeId = 500;
            Controls.RemoveGrade(gradeId);
            Grades grade = Controls.GetGrade(gradeId);
            Assert.Null(grade);
        }

        [Fact]
        public void UpdateCourse()
        {
            int courseId = 140;
            int gradeId = 89;
            Grades grade = Controls.GetGrade(gradeId);
            Controls.UpdateGradeCourse(grade, courseId);
            Grades updatedGrade = Controls.GetGrade(gradeId);
            Assert.True(updatedGrade.CourseId == courseId);
            
        }

        [Fact]
        public void UpdateDescription()
        {
        string description = "new updated description";
        int gradeId = 89;
        Grades grade = Controls.GetGrade(gradeId);
        Controls.UpdateGradeDescription(grade, description);
        Grades updatedGrade = Controls.GetGrade(gradeId);
        Assert.True(updatedGrade.Description == description);
        }

        [Fact]
        public void UpdateStudent()
        {
            int studentId = 81;
            int gradeId = 89;
            Grades grade = Controls.GetGrade(gradeId);
            Controls.UpdateGradeStudent(grade, studentId);
            Grades updatedGrade = Controls.GetGrade(gradeId);
            Assert.True(updatedGrade.StudentId == studentId);   
        }

        [Fact]
        public void UpdateTeacher()
        {
            int teacherId = 5;
            int gradeId = 89;
            Grades grade = Controls.GetGrade(gradeId);
            Controls.UpdateGradeTeacher(grade, teacherId);
            Grades updatedGrade = Controls.GetGrade(gradeId);
            Assert.True(updatedGrade.TeacherId == teacherId);   
        }
        
        [Fact]
        public void UpdateValue()
        {
            Random rnd = new Random();
            int value = rnd.Next(1,5);
            int gradeId = 89;
            Grades grade = Controls.GetGrade(gradeId);
            Controls.UpdateGradeValue(grade, value);
            Grades updatedGrade = Controls.GetGrade(gradeId);
            Assert.True(updatedGrade.Value == value);   
        }
    }
}