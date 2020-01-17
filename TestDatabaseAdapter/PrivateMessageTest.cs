using System;
using System.Collections.Generic;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    public class PrivateMessageTest
    {
        private DatabaseAdapter.OracleLib.OracleDatabaseControls Controls { get; } = new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        [Fact]
        public void CreateMessage()
        {
            User sender = Controls.GetUserById(69);
            User receiver = Controls.GetUserById(420);
            PrivateMessages message = new PrivateMessages(){Created = DateTime.Now, Content = "Message From Test Environment", ToUser = receiver, FromUser = sender};
            message.PmsgId = Controls.SendMessage(message);
            Assert.True(message.PmsgId > 0);
        }

        [Fact]
        public void GetAll()
        {
            List<PrivateMessages> messages = Controls.GetMessageAll();
            Assert.True(messages.Count > 0) ;
        }
        
        [Fact]
        public void GetById()
        {
            PrivateMessages messages = Controls.GetMessageById(1400);
            Assert.NotNull(messages);
        }

        [Fact]
        public void GetByUser()
        {
            User us = Controls.GetUserById(69);
            List<PrivateMessages> userMessages = Controls.GetMessageByUser(us);
            Assert.NotEmpty(userMessages);
        }

        [Fact]
        public void Remove()
        {
            int messId = 1400;
            Controls.RemoveMessage(messId);
            PrivateMessages messages = Controls.GetMessageById(messId);
            Assert.Null(messages);
        }
        
        
    }
}