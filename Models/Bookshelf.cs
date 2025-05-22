using Microsoft.AspNetCore.Mvc;

namespace BonLu.Models
{
    public class Bookshelf
    {
        public int Id { get; set; }

        // UserId corresponds to the IdentityUser.Id who created this book list
        public string? UserId { get; set; }

        // Navigation property for the enrollments
        public ICollection<Book> Books { get; set; } = new List<Book>();

    }
}
