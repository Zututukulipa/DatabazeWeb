using System;

namespace DatabaseAdapter.OracleLib.Models
{
    public class Files
    {

        public int FileId { get; set; }


        public int UserId { get; set; }


        public string FileName { get; set; }


        public string FileType { get; set; }


        public byte[] FileData { get; set; }


        public DateTime Created { get; set; }

    }
}
