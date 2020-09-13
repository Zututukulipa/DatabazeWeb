using System;

namespace DatabaseAdapter.OracleLib.Models
{
    public interface IPrivateMessages
    {
        int PmsgId { get; set; }
        User FromUser { get; set; }
        User ToUser { get; set; }
        string Content { get; set; }
        DateTime Created { get; set; }
    }
}