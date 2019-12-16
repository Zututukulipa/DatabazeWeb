using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Database.Entities
{
    public class LoginInfoFields
    {
        [Required]
        public string login { get; set; }
        [Required]
        [PasswordPropertyText]
        public string password { get; set; }
    }
}