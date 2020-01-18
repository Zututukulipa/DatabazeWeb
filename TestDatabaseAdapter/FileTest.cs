using System;
using System.Collections.Generic;
using System.IO;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using Xunit;

namespace TestDatabaseAdapter
{
    public class FileTest
    {
        private OracleDatabaseControls Controls { get; } =
            new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");



        [Fact]
        public void New()
        {
            
            string SourceLoc = "avatar_def.png";

            FileStream fs = new FileStream(SourceLoc, FileMode.Open, FileAccess.Read);
            byte[] imageData = new byte[fs.Length];
            fs.Read(imageData, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();
            Files image = new Files()
            {
                Created = DateTime.Today,
                FileData = imageData,
                FileName = SourceLoc,
                UserId = 5,
                FileType = "png",
                
            };
            image.FileId = Controls.InsertImage(image);
            Assert.True(image.FileId > 0);
        }

        [Fact]
        public void Remove()
        {
            int pictureId = 1;
            Controls.RemoveFile(pictureId);
            Files file = Controls.GetFile(pictureId);
            Assert.Null(file);
        }

        [Fact]
        public void GetAll()
        {
            List<Files> files = Controls.GetFileAll();
            Assert.NotEmpty(files);
        }

        [Fact]
        public void GetById()
        {
            int fileId = 1;
            Files file = Controls.GetFile(fileId);
            Assert.NotNull(file);
        }

        [Fact]
        public void GetByUser()
        {
            int userId = 5;
            User user = Controls.GetUserById(userId);
            List<Files> files = Controls.GetFile(user);
            Assert.NotEmpty(files);
            
        }
    }
}