using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BonLu.Models;

namespace BonLu
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<FriendsList> Friends { get; set; }
        public DbSet<Bookshelf> Bookshelfs { get; set; }
    }
}