using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class BookInputDto
    {
        [Required(ErrorMessage = "Book Title is a required field.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Book Genre is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Genre is 60 characters.")]
        public string Genre { get; set; }

        public string Edition { get; set; }

        [Required(ErrorMessage = "Price is a required field.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "ISBN is a required field.")]
        [MaxLength(13, ErrorMessage = "Maximum length for the ISBN is 13 characters.")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Publisher is a required field.")]
        public string Publisher { get; set; }

        [Required(ErrorMessage = "Year of Publication is a required field.")]
        public int YearPublished { get; set; }

        [Required(ErrorMessage = "Author(s) is a required field.")]
        public List<Author> Authors { get; set; }
    }
}
