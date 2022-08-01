using System;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class AuthorOutputDto
    {
        public string Name { get; set; }

        public List<Book> Books { get; set; }
    }
}
