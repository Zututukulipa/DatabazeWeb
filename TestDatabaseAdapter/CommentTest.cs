using System;
using System.Collections.Generic;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
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
            List<Comments> comments = Controls.GetComments();
            Assert.NotEmpty(comments);
        }

        [Fact]
        public void GetById()
        {
            int commentId = 2;
            Comments comment = Controls.GetCommentById(commentId);
            Assert.NotNull(comment);
        }

        [Fact]
        public void GetByMessage()
        {
            int messageId = 24;
            List<Comments> comments = Controls.GetCommentsByMessage(messageId);
            Assert.NotEmpty(comments);
        }

        [Fact]
        public void GetByUser()
        {
            User user = Controls.GetUserById(484);
            List<Comments> comments = Controls.GetCommentsByUser(user);
            Assert.NotEmpty(comments);
        }

        [Fact]
        public void Remove()
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

            Controls.RemoveCommentById(gMessageId);

            Comments deletedCommentCheck = Controls.GetCommentById(gMessageId);
            Assert.Null(deletedCommentCheck);

        }

        [Fact]
        public void UpdateContent()
        {
            Random rnd = new Random();
            string messageCheck = "UpdatedComment Contents...";
            
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

            Controls.UpdateCommentContent(comment, messageCheck);

            Comments flag = Controls.GetCommentById(comment.CommentId);
            Assert.True(flag.Content == messageCheck);
        }

        [Fact]
        public void UpdateMessage()
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

            Controls.UpdateCommentPost(comment.CommentId, 1);

            Comments flag = Controls.GetCommentById(comment.CommentId);
            Assert.True(flag.MessageId == 1);
        }

        [Fact]
        public void UpdateUser()
        {
            Random rnd = new Random();
            
            Group group = Controls.GetGroupById(1);
            List<GroupMessages> gMessages = Controls.GetGroupMessageByGroup(group);
            List<Students> students = Controls.GetGroupUsers(group.GroupId);
            
            int gMessageId = rnd.Next(gMessages.Count);
            Students contentOwner = students[rnd.Next(students.Count)];
            Students newContentOwner = students[rnd.Next(students.Count)];
            Comments comment = new Comments()
            {
                ContentOwner = contentOwner,
                Content = "Test added comment",
                Created = DateTime.Now,
                MessageId = gMessageId,
                UserId = contentOwner.UserId
            };
            comment.CommentId = Controls.InsertComment(comment, gMessages[gMessageId].GmsgId);

            Controls.UpdateCommentOwner(comment.CommentId, newContentOwner.UserId);

            Comments flag = Controls.GetCommentById(comment.CommentId);
            Assert.True(flag.ContentOwner.Equals(newContentOwner));
        }
        
    }
}