using System;

namespace DatabaseAdapter.OracleLib.Models
{
    public class GroupMessages
    {

        public int GmsgId { get; set; }


        public int GroupId { get; set; }


        public int UserId { get; set; }


        public string Content { get; set; }


        public DateTime Created { get; set; }

    }
}
