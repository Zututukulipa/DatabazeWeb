using System;
using System.Collections.Generic;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    public class CommentTest
    {
        private OracleDatabaseControls Controls { get; } = new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        [Fact]
        public void CreateComment()
        {
            var rnd = new Random();
            for (var i = 0; i < 60; i++)
            {
                var group = Controls.GetGroupById(rnd.Next(1,20));
                var gMessages = Controls.GetGroupMessageByGroup(group);
                var students = Controls.GetGroupUsers(group.GroupId);
                if (gMessages.Count > 0 && students.Count > 0)
                {
                    var gMessageId = rnd.Next(1, gMessages.Count);
                    var contentOwner = students[rnd.Next( students.Count)];
                    var comment = new Comments
                    {
                        ContentOwner = contentOwner,
                        Content = "Test added comment",
                        Created = DateTime.Now,
                        MessageId = gMessageId,
                        UserId = contentOwner.UserId
                    };
                    comment.CommentId = Controls.InsertComment(comment, gMessages[gMessageId].GmsgId);
                }
            }
           

        }

        [Fact]
        public void GetAll()
        {
            var comments = Controls.GetComments();
            Assert.NotEmpty(comments);
        }

        [Fact]
        public void GetById()
        {
            var commentId = 2;
            var comment = Controls.GetCommentById(commentId);
            Assert.NotNull(comment);
        }

        [Fact]
        public void GetByMessage()
        {
            var messageId = 24;
            var comments = Controls.GetCommentsByMessage(messageId);
            Assert.NotEmpty(comments);
        }

        [Fact]
        public void GetByUser()
        {
            var user = Controls.GetUserById(484);
            var comments = Controls.GetCommentsByUser(user);
            Assert.NotEmpty(comments);
        }

        [Fact]
        public void Remove()
        {
            var rnd = new Random();
            
            var group = Controls.GetGroupById(1);
            var gMessages = Controls.GetGroupMessageByGroup(group);
            var students = Controls.GetGroupUsers(group.GroupId);
            
            var gMessageId = rnd.Next(gMessages.Count);
            var contentOwner = students[rnd.Next(students.Count)];
            var comment = new Comments
            {
                ContentOwner = contentOwner,
                Content = "Test added comment",
                Created = DateTime.Now,
                MessageId = gMessageId,
                UserId = contentOwner.UserId
            };
            comment.CommentId = Controls.InsertComment(comment, gMessages[gMessageId].GmsgId);

            Controls.RemoveCommentById(gMessageId);

            var deletedCommentCheck = Controls.GetCommentById(gMessageId);
            Assert.Null(deletedCommentCheck);

        }

        [Fact]
        public void UpdateContent()
        {
            var rnd = new Random();
            var messageCheck = "UpdatedComment Contents...";
            
            var group = Controls.GetGroupById(1);
            var gMessages = Controls.GetGroupMessageByGroup(group);
            var students = Controls.GetGroupUsers(group.GroupId);
            
            var gMessageId = rnd.Next(gMessages.Count);
            var contentOwner = students[rnd.Next(students.Count)];
            var comment = new Comments
            {
                ContentOwner = contentOwner,
                Content = "Test added comment",
                Created = DateTime.Now,
                MessageId = gMessageId,
                UserId = contentOwner.UserId
            };
            comment.CommentId = Controls.InsertComment(comment, gMessages[gMessageId].GmsgId);

            Controls.UpdateCommentContent(comment, messageCheck);

            var flag = Controls.GetCommentById(comment.CommentId);
            Assert.True(flag.Content == messageCheck);
        }

        [Fact]
        public void UpdateMessage()
        {
            var rnd = new Random();
            
            var group = Controls.GetGroupById(1);
            var gMessages = Controls.GetGroupMessageByGroup(group);
            var students = Controls.GetGroupUsers(group.GroupId);
            
            var gMessageId = rnd.Next(gMessages.Count);
            var contentOwner = students[rnd.Next(students.Count)];
            var comment = new Comments
            {
                ContentOwner = contentOwner,
                Content = "Test added comment",
                Created = DateTime.Now,
                MessageId = gMessageId,
                UserId = contentOwner.UserId
            };
            comment.CommentId = Controls.InsertComment(comment, gMessages[gMessageId].GmsgId);

            Controls.UpdateCommentPost(comment.CommentId, 1);

            var flag = Controls.GetCommentById(comment.CommentId);
            Assert.True(flag.MessageId == 1);
        }

        [Fact]
        public void UpdateUser()
        {
            var rnd = new Random();
            
            var group = Controls.GetGroupById(1);
            var gMessages = Controls.GetGroupMessageByGroup(group);
            var students = Controls.GetGroupUsers(group.GroupId);
            
            var gMessageId = rnd.Next(gMessages.Count);
            var contentOwner = students[rnd.Next(students.Count)];
            var newContentOwner = students[rnd.Next(students.Count)];
            var comment = new Comments
            {
                ContentOwner = contentOwner,
                Content = "Test added comment",
                Created = DateTime.Now,
                MessageId = gMessageId,
                UserId = contentOwner.UserId
            };
            comment.CommentId = Controls.InsertComment(comment, gMessages[gMessageId].GmsgId);

            Controls.UpdateCommentOwner(comment.CommentId, newContentOwner.UserId);

            var flag = Controls.GetCommentById(comment.CommentId);
            Assert.True(flag.ContentOwner.Equals(newContentOwner));
        }
        
    }
}