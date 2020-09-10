using System;
using System.Collections.Generic;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    public class GroupMessageTest
    {
        private OracleDatabaseControls Controls { get; } = new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        [Fact]
        public void Create()
        {
            var testGroup = Controls.GetGroupById(1);
            var teacher = Controls.GetTeacherById(testGroup.TeacherId);
            var groupMessage = new GroupMessages {Content = "Testing Message, waiting for some comments to come", Created = DateTime.Now, GroupId = testGroup.GroupId, UserId = teacher.UserId};
            groupMessage.GmsgId = Controls.SendGroupMessage(groupMessage);
            Assert.True(groupMessage.GmsgId >= 0);
        }

        [Fact]
        public void GetAll()
        {
            var groupMessages = Controls.GetGroupMessageAll();
            Assert.NotEmpty(groupMessages);
        }
        
        [Fact]
        public void GetByGroupAll()
        {
            var group = Controls.GetGroupById(1);
            var groupMessages = Controls.GetGroupMessageByGroup(group);
            Assert.NotEmpty(groupMessages);
        }

        [Fact]
        public void GetById()
        {
            var message = Controls.GetGroupMessageById(1);
            Assert.NotNull(message);
        }

        [Fact]
        public void GetByUser()
        {
            var groupMessages = Controls.GetGroupMessagesByUser(4);
            Assert.NotEmpty(groupMessages);
        }

        [Fact]
        public void UpdateContent()
        {
            var updatedContent = "New, better, updated content awaiting comments..";
            Controls.UpdateGroupMessageContent(Controls.GetGroupMessageById(1), updatedContent);
            var updated = Controls.GetGroupMessageById(1);
            Assert.True(updated.Content == updatedContent);
        }
        [Fact]
        public void Remove()
        {
            var testGroup = Controls.GetGroupById(1);
            var teacher = Controls.GetTeacherById(testGroup.TeacherId);
            var groupMessage = new GroupMessages {Content = "Testing Message, waiting for removal", Created = DateTime.Now, GroupId = testGroup.GroupId, UserId = teacher.UserId};
            groupMessage.GmsgId = Controls.SendGroupMessage(groupMessage);
            Controls.RemoveGroupMessage(groupMessage.GmsgId);
            var flag = Controls.GetGroupMessageById(groupMessage.GmsgId);
            Assert.Null(flag);
        }

        [Fact]
        public void UpdateGroup()
        {
            var testGroup = Controls.GetGroupById(1);
            var teacher = Controls.GetTeacherById(testGroup.TeacherId);
            var groupMessage = new GroupMessages {Content = "Testing Message, waiting for group change from group 1", Created = DateTime.Now, GroupId = testGroup.GroupId, UserId = teacher.UserId};
            groupMessage.GmsgId = Controls.SendGroupMessage(groupMessage);
            Controls.UpdateGroupMessageGroup(groupMessage.GmsgId, 22);
            var flag = Controls.GetGroupMessageById(groupMessage.GmsgId);
            Assert.True(flag.GroupId == 22);
        }
        
        [Fact]
        public void UpdateGroupMessageOwner()
        {
            var testGroup = Controls.GetGroupById(1);
            var users = Controls.GetGroupUsers(1);
            var random = new Random();
            var teacher = Controls.GetTeacherById(testGroup.TeacherId);
            var groupMessage = new GroupMessages {Content = "Testing Message, waiting for group change from group 1", Created = DateTime.Now, GroupId = testGroup.GroupId, UserId = teacher.UserId};
            groupMessage.GmsgId = Controls.SendGroupMessage(groupMessage);
            var newUser = random.Next(users.Count);
            Controls.UpdateGroupMessageOwner(groupMessage.GmsgId, newUser);
            var flag = Controls.GetGroupMessageById(groupMessage.GmsgId);
            Assert.True(flag.UserId == newUser);
        }
        
    }
}