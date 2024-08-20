using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Data.Entities
{
    public class Author
    {
        public int AuthorId { get; set; }
        public required string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
