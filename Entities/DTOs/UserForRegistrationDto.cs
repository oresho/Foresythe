using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class UserForRegistrationDto
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "PhoneNumber is required")]
        public string PhoneNumber { get; set; }

        public string Role { get; set; }
    }
}
