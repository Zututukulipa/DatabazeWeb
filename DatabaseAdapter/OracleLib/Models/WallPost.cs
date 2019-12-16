using System;
using System.Collections.Generic;
using DatabaseAdapter.OracleLib.Models;

namespace DatabaseAdapter.OracleLib.WallPosts
{
    public class WallPost
    {

        public int GmsgId { get; set; }


        public int GroupId { get; set; }

        
        public User Owner { get; set; }


        public string MessageContent { get; set; }
        
        public List<Comments> Comments { get; set; }


        public DateTime Created { get; set; }

        public WallPost GetPostById(int id)
        {


            return null;
        }

        public WallPost(User owner)
        {
            Created = DateTime.Now;
            MessageContent = "Generic message";
            Owner = owner;
            Comments = new List<Comments>();
        }
        
        public WallPost()
        {
        }
    }
}