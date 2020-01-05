using System.Collections.Generic;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Enums;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    public class UserTest
    {
        private OracleDatabaseControls Controls { get; } =
            new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        [Fact]
        public void Create()
        {
            User user = new User(){Admin = false, Bio = "Ideal Bio", Email = "st40@schoold.gg", Password = "heslo", Role = Roles.Professor, Username = "testUname", FirstName = "Johan", MiddleName = "Siegfried", LastName = "Muller", StatusId = 1};
            user.UserId = Controls.InsertUser(user);
            var u = Controls.GetUserById(user.UserId);
            Assert.True(u.UserId == user.UserId);
        }

        [Fact]
        public void GetAll()
        {
            List<User> users = Controls.GetUserAll();
            Assert.True(users.Count > 1);
        }

        [Fact]
        public void GetUser()
        {
            User user = Controls.GetUserById(1);
            Assert.NotNull(user);
        }

        [Fact]
        public void Login()
        {
            //FUNCTION LOGIN(p_username USERS.USERNAME%TYPE, p_password USERS.PASSWORD%TYPE) RETURN USERS.USER_ID%TYPE IS
            // v_id USERS.USER_ID%TYPE;
            // v_status USERS.STATUS_ID%TYPE;
            User user = Controls.GetUserById(1);
            User us = Controls.Login("vrana", "ucitel");
            Assert.True(user.UserId == us.UserId);
        }

        [Fact]
        public void Update_Admin()
        {
            User user = Controls.GetUserById(1);
            Controls.SetUserAdmin(user, true);
            Assert.True(user.Admin);
        }

        [Fact]
        public void Update_Details()
        {
            User user = Controls.GetUserById(1);
            user.Bio = "UPDATED FuCkInG BiO";
            Controls.UpdateUser(user);
            User u2 = Controls.GetUserById(1);
            Assert.True(user.Bio != u2.Bio);
        }

        [Fact]
        public void Update_Login()
        {
            Controls.UpdateLogin(1, "PrvniUzivatel", "prvniHeslo");
        }

        [Fact]
        public void Update_Status()
        {
            Controls.UpdateUserStatus(1, 0);
        }
    }
}