using System;
using DatabaseAdapter.OracleLib.Models;

public class Comments
{

    public string CommentId { get; set; }

    public  User ContentOwner { get; set; }
    public string UserId { get; set; }


    public string MessageId { get; set; }


    public string Content { get; set; }


    public DateTime Created { get; set; }

}
