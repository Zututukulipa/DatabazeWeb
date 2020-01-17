using System;
using System.Collections.Generic;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    //TODO finish tests
    public class CommentTest
    {
        private DatabaseAdapter.OracleLib.OracleDatabaseControls Controls { get; } = new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        [Fact]
        public void CreateComment()
        {
            Random rnd = new Random();
            
            Group group = Controls.GetGroupById(1);
            List<GroupMessages> gMessages = Controls.GetGroupMessageByGroup(group);
            List<Students> students = Controls.GetGroupUsers(group.GroupId);
            
            int gMessageId = rnd.Next(gMessages.Count);
            Students contentOwner = students[rnd.Next(students.Count)];
            Comments comment = new Comments()
            {
                ContentOwner = contentOwner,
                Content = "Test added comment",
                Created = DateTime.Now,
                MessageId = gMessageId,
                UserId = contentOwner.UserId
            };
            comment.CommentId = Controls.InsertComment(comment, gMessages[gMessageId].GmsgId);
            Assert.True(comment.CommentId > -1);

        }

        [Fact]
        public void GetAll()
        {
            
        }
        
    }
}