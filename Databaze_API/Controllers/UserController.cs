using System.Net;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Databaze_API.Controllers
{
    public class UserController
    {
        public static User ActiveUser { get; set; }
        private string ConnectionString { get; }

        private OracleDatabaseControls _controls;
        

        public void Login(string logonLogin, string logonPassword)
        {
            ActiveUser = _controls.Login(logonLogin, logonPassword);
        }

        public UserController()
        {
            ConnectionString = "DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;";
            _controls = new OracleDatabaseControls(ConnectionString);
            ActiveUser = null;
        }
        
        
        
    }
}