namespace BookStore.API.DTOs
{
    public class BookDto
    {
        public int BookId { get; set; }
        public required string Title { get; set; }
        public string Genre { get; set; }
        public DateTime PublicationDate { get; set; }
        public int AuthorId { get; set; }
    }
}
