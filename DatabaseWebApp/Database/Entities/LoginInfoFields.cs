using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class LoginInfoFields
    {
        [Required]
        public string Login { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}