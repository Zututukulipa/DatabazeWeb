using System.Collections.Generic;
using System.Security.Claims;
using DatabaseAdapter.OracleLib.Models;
using Databaze_API.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Database.Services
{
    public class UserService : Controller
    {
        public static User ActiveUser { get; set; }
        public UserController UserController { get; }

        public UserService()
        {
         UserController = new UserController();   
        }
        public void Authenticate(string email, string password)
        {
            var user = UserController.Login(email, password);

            if (user != null)
            {
                var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.GivenName, user.MiddleName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                };
                
                var userIdentity = new ClaimsIdentity(userClaims);
                var userPrincipal = new ClaimsPrincipal(new []{userIdentity});

                HttpContext context = new DefaultHttpContext();
                context.SignInAsync(userPrincipal);

            }




        }
    }
}