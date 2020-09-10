using System;
using System.Collections.Generic;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    public class PrivateMessageTest
    {
        private OracleDatabaseControls Controls { get; } = new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        [Fact]
        public void CreateMessage()
        {
            var sender = Controls.GetUserById(69);
            var receiver = Controls.GetUserById(420);
            var message = new PrivateMessages {Created = DateTime.Now, Content = "Message From Test Environment", ToUser = receiver, FromUser = sender};
            message.PmsgId = Controls.SendMessage(message);
            Assert.True(message.PmsgId > 0);
        }

        [Fact]
        public void GetAll()
        {
            var messages = Controls.GetMessageAll();
            Assert.True(messages.Count > 0) ;
        }
        
        [Fact]
        public void GetById()
        {
            var messages = Controls.GetMessageById(1400);
            Assert.NotNull(messages);
        }

        [Fact]
        public void GetByUser()
        {
            var us = Controls.GetUserById(69);
            var userMessages = Controls.GetMessageByUser(us);
            Assert.NotEmpty(userMessages);
        }

        [Fact]
        public void Remove()
        {
            var messId = 1400;
            Controls.RemoveMessage(messId);
            var messages = Controls.GetMessageById(messId);
            Assert.Null(messages);
        }
        
        
    }
}