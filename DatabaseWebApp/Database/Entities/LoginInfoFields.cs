using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    internal class LoginInfoFields
    {
        [Required]
        public string Login { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}