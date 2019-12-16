using DatabaseAdapter.OracleLib.Models;
using Databaze_API.Controllers;

namespace Database.Services
{
    public class UserService
    {
        public User ActiveUser { get; set; } = UserController.ActiveUser;
        public UserController Controller { get; } = new UserController();

    }
}