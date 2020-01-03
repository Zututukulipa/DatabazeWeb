using System;

namespace DatabaseAdapter.OracleLib.Models
{
    public class Comments
    {

        public int CommentId { get; set; }

        public  User ContentOwner { get; set; }
        public int UserId { get; set; }


        public int MessageId { get; set; }


        public string Content { get; set; }


        public DateTime Created { get; set; }

        public Comments()
        {
        }

        public Comments(User contentOwner, string content, DateTime created)
        {
            ContentOwner = contentOwner;
            Content = content;
            Created = created;
        }
    }
}
