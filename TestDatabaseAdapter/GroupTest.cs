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
            Group group = new Group(){Name = "Test Group", ActualCapacity = 0, MaxCapacity = 20, TeacherId = 4};
            group.GroupId = Controls.InsertGroup(group);
            Group returnedGroup = Controls.GetGroupById(1);
            Assert.True(group.GroupId == returnedGroup.GroupId);
        }

        [Fact]
        public void AddStudent()
        {
            Random rnd = new Random();
            for (int i = 0; i < 20; i++)
            {
                Controls.InsertStudentIntoGroup(Controls.GetStudentById(rnd.Next(50,500)), 1);
            }
            
            
        }
        
    }
    
    
}