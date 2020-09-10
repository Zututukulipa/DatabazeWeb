using System;
using System.Collections.Generic;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
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
            var us = _controls.GetUserById(300);
            var messages = _controls.GetMessageAll();
            var messagesByUser = _controls.GetMessageByUser(us);
            var messageById = _controls.GetMessageById(140);
            
        }

        [Fact]
        public void MessagingIntegration()
        {
            var rnd = new Random();

            var users = _controls.GetUserAll();
            string[] messages = {
                "wassup", "Hey", "Hru babe?", "I always wanted to tell you", "that I hate you", "that I miss you",
                "We have a test tomorrow", "Or was it yesterday?", "Can you please send me the test?", "Ok, sure", 
                "I am on my way", ":D", "KkkKKKKkkkKKKkkKKKk", "for real tho", ":)", "I really need your help", "Can you please hit me up in the class?",
                "I really dont know what else to say", "The whole story is", "that somehow I managed to make it"
            };
            for (var j = 0; j < 50; j++)
            {
               var pmessages = new List<PrivateMessages>();
            
                for (var i = 0; i < 100; i++)
                {
                    var x = rnd.Next(users.Count);
                    if (j != x)
                    {
                        var msg = new PrivateMessages();
                        msg.Content = messages[rnd.Next(messages.Length)];
                        msg.Created = DateTime.Now;
                        msg.FromUser = users[j];
                        msg.ToUser = users[x];
                        var id = _controls.SendMessage(msg);
                        msg.PmsgId = id;
                        pmessages.Add(msg);
                    }
                }
            
                foreach (var mess in users)
                {
                    var userRecipientsMessages = pmessages.FindAll((privateMessages => mess.Equals(privateMessages.ToUser)));
                    users[j].UserConversations.Add(mess, userRecipientsMessages);
                }
            }

           

        }

        [Fact]
        public void GetGroupMesages()
        {
            var rnd = new Random();
            string[] gmsgs =
            {
                "Important announcement! All of the students, please message me for details",
                "Next lesson is cancelled.", "New test added to the course",
                "Next lesson we are having a test", "I think that you need to improve your results",
                "I am pleasantly surprised", "Hello, I am your new teacher"
            };
            var groups = _controls.GetGroupAll();
            
            foreach (var _ in groups)
            {
                for (var i = 0; i < 5; i++)
                {
                    _controls.SendGroupMessage(new GroupMessages
                    {
                        Content = gmsgs[rnd.Next(gmsgs.Length)],
                        Created = DateTime.Now,
                        GroupId = rnd.Next(20),
                        UserId = rnd.Next(300)
                    });
                }
            }

        }
    }
}