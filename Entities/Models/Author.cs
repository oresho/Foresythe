using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Author
    {
        [Column("AuthorId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Author Name is a required field.")]
        public string Name { get; set; }

        public List<Book> Books { get; set; }

    }
}