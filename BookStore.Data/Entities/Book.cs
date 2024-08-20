using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Data.Entities
{
    public class Book
    {
        public int BookId { get; set; }
        public required string Title { get; set; }
        public required string Genre { get; set; }
        public DateTime PublicationDate { get; set; }

        public int AuthorId { get; set; }
        public required Author Author { get; set; }
    }
}
