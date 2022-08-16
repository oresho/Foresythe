using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class AuthorInputDto
    {
        [Required(ErrorMessage = "First Name is a required field.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is a required field.")]
        public string LastName { get; set; }

        public List<BookInputDto> Books { get; set; }

    }
}
