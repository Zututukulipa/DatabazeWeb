using System;
using System.Collections.Generic;
using System.IO;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
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
            
            var sourceLoc = "avatar_def.png";

            var fs = new FileStream(sourceLoc, FileMode.Open, FileAccess.Read);
            var imageData = new byte[fs.Length];
            fs.Read(imageData, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            var image = new Files
            {
                Created = DateTime.Today,
                FileData = imageData,
                FileName = sourceLoc,
                UserId = 5,
                FileType = "png"
            };
            image.FileId = Controls.InsertImage(image);
            Assert.True(image.FileId > 0);
        }

        [Fact]
        public void Remove()
        {
            var pictureId = 1;
            Controls.RemoveFile(pictureId);
            var file = Controls.GetFile(pictureId);
            Assert.Null(file);
        }

        [Fact]
        public void GetAll()
        {
            var files = Controls.GetFileAll();
            Assert.NotEmpty(files);
        }

        [Fact]
        public void GetById()
        {
            var fileId = 1;
            var file = Controls.GetFile(fileId);
            Assert.NotNull(file);
        }

        [Fact]
        public void GetByUser()
        {
            var userId = 5;
            var user = Controls.GetUserById(userId);
            var files = Controls.GetFile(user);
            Assert.NotEmpty(files);
            
        }
    }
}