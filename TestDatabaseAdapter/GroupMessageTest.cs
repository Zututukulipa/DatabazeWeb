using System;
using System.Collections.Generic;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    public class GroupMessageTest
    {
        private DatabaseAdapter.OracleLib.OracleDatabaseControls Controls { get; } = new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        [Fact]
        public void Create()
        {
            Group testGroup = Controls.GetGroupById(1);
            Teachers teacher = Controls.GetTeacherById(testGroup.TeacherId);
            GroupMessages groupMessage = new GroupMessages(){Content = "Testing Message, waiting for some comments to come", Created = DateTime.Now, GroupId = testGroup.GroupId, UserId = teacher.UserId};
            groupMessage.GmsgId = Controls.SendGroupMessage(groupMessage);
            Assert.True(groupMessage.GmsgId >= 0);
        }

        [Fact]
        public void GetAll()
        {
            List<GroupMessages> groupMessages = Controls.GetGroupMessageAll();
            Assert.NotEmpty(groupMessages);
        }
        
        [Fact]
        public void GetByGroupAll()
        {
            Group group = Controls.GetGroupById(1);
            List<GroupMessages> groupMessages = Controls.GetGroupMessageByGroup(group);
            Assert.NotEmpty(groupMessages);
        }

        [Fact]
        public void GetById()
        {
            GroupMessages message = Controls.GetGroupMessageById(1);
            Assert.NotNull(message);
        }

        [Fact]
        public void GetByUser()
        {
            List<GroupMessages> groupMessages = Controls.GetGroupMessagesByUser(4);
            Assert.NotEmpty(groupMessages);
        }

        [Fact]
        public void UpdateContent()
        {
            string updatedContent = "New, better, updated content awaiting comments..";
            Controls.UpdateGroupMessageContent(Controls.GetGroupMessageById(1), updatedContent);
            GroupMessages updated = Controls.GetGroupMessageById(1);
            Assert.True(updated.Content == updatedContent);
        }
        [Fact]
        public void Remove()
        {
            Group testGroup = Controls.GetGroupById(1);
            Teachers teacher = Controls.GetTeacherById(testGroup.TeacherId);
            GroupMessages groupMessage = new GroupMessages(){Content = "Testing Message, waiting for removal", Created = DateTime.Now, GroupId = testGroup.GroupId, UserId = teacher.UserId};
            groupMessage.GmsgId = Controls.SendGroupMessage(groupMessage);
            Controls.RemoveGroupMessage(groupMessage.GmsgId);
            GroupMessages flag = Controls.GetGroupMessageById(groupMessage.GmsgId);
            Assert.Null(flag);
        }

        [Fact]
        public void UpdateGroup()
        {
            Group testGroup = Controls.GetGroupById(1);
            Teachers teacher = Controls.GetTeacherById(testGroup.TeacherId);
            GroupMessages groupMessage = new GroupMessages(){Content = "Testing Message, waiting for group change from group 1", Created = DateTime.Now, GroupId = testGroup.GroupId, UserId = teacher.UserId};
            groupMessage.GmsgId = Controls.SendGroupMessage(groupMessage);
            Controls.UpdateGroupMessageGroup(groupMessage.GmsgId, 22);
            GroupMessages flag = Controls.GetGroupMessageById(groupMessage.GmsgId);
            Assert.True(flag.GroupId == 22);
        }
        
        [Fact]
        public void UpdateGroupMessageOwner()
        {
            Group testGroup = Controls.GetGroupById(1);
            List<Students> users = Controls.GetGroupUsers(1);
            Random random = new Random();
            Teachers teacher = Controls.GetTeacherById(testGroup.TeacherId);
            GroupMessages groupMessage = new GroupMessages(){Content = "Testing Message, waiting for group change from group 1", Created = DateTime.Now, GroupId = testGroup.GroupId, UserId = teacher.UserId};
            groupMessage.GmsgId = Controls.SendGroupMessage(groupMessage);
            int newUser = random.Next(users.Count);
            Controls.UpdateGroupMessageOwner(groupMessage.GmsgId, newUser);
            GroupMessages flag = Controls.GetGroupMessageById(groupMessage.GmsgId);
            Assert.True(flag.UserId == newUser);
        }
        
    }
}