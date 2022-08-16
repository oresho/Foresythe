using System.Collections.Generic;

namespace Entities.DTOs
{
    public class BookOutputDto
    {
        public string Title { get; set; }
        public string Edition { get; set; }
        public double Price { get; set; }
        public int YearPublished { get; set; }
        public List<string> AuthorsNames { get; set; }
    }
}
