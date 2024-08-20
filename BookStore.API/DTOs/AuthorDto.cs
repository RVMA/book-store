namespace BookStore.API.DTOs
{
    public class AuthorDto
    {
        public int AuthorId { get; set; }
        public required string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
    }
}
