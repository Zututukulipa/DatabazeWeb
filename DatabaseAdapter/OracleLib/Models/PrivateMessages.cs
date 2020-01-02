using System;

namespace DatabaseAdapter.OracleLib.Models
{
    public class PrivateMessages
    {

        public int PmsgId { get; set; }


        public User FromUser { get; set; }


        public User ToUser { get; set; }


        public string Content { get; set; }


        public DateTime Created { get; set; }

        public PrivateMessages(User user, string content)
        {
            ToUser = user;
            Content = content;
            Created = DateTime.Now;
        
        }

        public PrivateMessages(User userAddressedTo)
        {
            ToUser = userAddressedTo;
            FromUser = new User();
            Content = "Generic message";
            Created = DateTime.Now;
        }

        public PrivateMessages()
        {
        }
    }
}
