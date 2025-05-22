using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BonLu.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BonLu;
using Microsoft.Win32;
using Microsoft.AspNetCore.RateLimiting;

[ApiController]
[Route("api/[controller]")]
public class AdminActionsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminActionsController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Only admins can create books.
    [Authorize(Roles = "Admin")]
    [HttpPost]
    
    public async Task<IActionResult> CreateBook([FromBody] DataTransferForBook dt)
    {
        var book = new Book
        {
            Title = dt.Title,
            Id = dt.Id,
            ISBN = dt.ISBN,
            Author = dt.Author,
            Pages = dt.Pages
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return Ok(book); // optionally return a simplified response too
    }

    // Only admins can edit books
    [Authorize(Roles = "Admin")]
    [HttpPost("/editbook/{isbn}")]

    public async Task<IActionResult> EditBook(string isbn, string title = "Your Title", string author = "Author", int pages = 0)
    {
        var book = _context.Books.FirstOrDefault(b => b.ISBN == isbn);

        if (title != "Your Title") { book.Title = title; }

        if (author != "Author") { book.Author = author; }

        if (pages > 0) { book.Pages = pages; }

        _context.SaveChanges();

        return Ok(book);
    }

    // Only admins can delete books.
    [Authorize(Roles = "Admin")]
    [HttpDelete("{isbn}")]
    public async Task<IActionResult> DeleteBook(string isbn)
    {
        var book = await _context.Books.FirstOrDefaultAsync(i => i.ISBN == isbn);

        if (book == null)
        {
            return NotFound();
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return Ok(book);
    }
}
