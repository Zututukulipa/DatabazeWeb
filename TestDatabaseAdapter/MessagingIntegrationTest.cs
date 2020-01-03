using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Database.Data;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Oracle.ManagedDataAccess.Client;
using Xunit;

namespace TestDatabaseAdapter
{
    public class MessagingIntegrationTest
    {
        private readonly OracleDatabaseControls _controls =
            new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        [Fact]
        public void GetAll()
        {
            User us = _controls.GetUserById(300);
            List<PrivateMessages> messages = _controls.GetMessageAll();
            List<PrivateMessages> messagesByUser = _controls.GetMessageByUser(us);
            PrivateMessages messageById = _controls.GetMessageById(140);
            
        }

        [Fact]
        public void MessagingIntegration()
        {
            var users = DataGen.getDefaultUser(50);
            string[] messages = new[]
            {
                "wassup", "Hey", "Hru babe?", "I always wanted to tell you", "that I hate you", "that I miss you",
                "We have a test tomorrow", "Or was it yesterday?"
            };
            Random rnd = new Random();
            for (int j = 0; j < 50; j++)
            {
               List<PrivateMessages> pmessages = new List<PrivateMessages>();
            
                for (int i = 0; i < 500; i++)
                {
                    var x = rnd.Next(users.Count);
                    if (j != x)
                    {
                        var msg = new PrivateMessages();
                        msg.Content = messages[rnd.Next(messages.Length)];
                        msg.Created = DateTime.Now;
                        msg.FromUser = users[j];
                        msg.ToUser = users[x];
                        int id = _controls.SendMessage(msg);
                        msg.PmsgId = id;
                        pmessages.Add(msg);
                    }
                }
            
                foreach (var mess in users)
                {
                    var hjfkda = pmessages.FindAll((privateMessages => mess.Equals(privateMessages.ToUser)));
                    users[j].UserConversations.Add(mess, hjfkda);
                }
            }

            var testMessage = _controls.GetMessage(200);

            Assert.True(testMessage.FromUser.UserId == 30);
            _controls.RemoveMessage(testMessage);
        }
    }
}