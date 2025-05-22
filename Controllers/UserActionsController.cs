using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BonLu.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BonLu;
using Microsoft.Win32;

[ApiController]
[Route("api/[controller]")]
public class UserActionController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserActionController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Only users can add books. DNF
    [Authorize(Roles = "User")]
    [HttpPost("/AddBook/{isbn}")]
    public async Task<IActionResult> AddBookToShelf(string isbn)
    {
        var userId = _userManager.GetUserId(User);
        Bookshelf usersBookshelf = _context.Bookshelfs.FirstOrDefault(s => s.UserId == userId);

        // Create bookshelf if user doesnt have one
        if (usersBookshelf == null)
        {
            usersBookshelf = new Bookshelf
            {
                UserId = userId
            };
            _context.Bookshelfs.Add(usersBookshelf);
            await _context.SaveChangesAsync();
        }

        var book = usersBookshelf.Books.FirstOrDefault(b => b.ISBN == isbn);

        if (book == null)
        {
            Console.WriteLine("A");
            usersBookshelf.Books.Add(_context.Books.FirstOrDefault(b => b.ISBN == isbn));
        }

        
        return Ok(
            await _context.Bookshelfs
            .Where(s => s.UserId == userId)
            .Include(c => c.Books)
            .ToListAsync()
            ); // optionally return a simplified response too
    }

    // Only users can add books. DNF
    [Authorize(Roles = "User")]
    [HttpPost("/SeeMyBookshelf")]
    public async Task<IActionResult> SeeShelf()
    {
        var userId = _userManager.GetUserId(User);
        var bookshelf = await _context.Bookshelfs
            .Where(s => s.UserId == userId)
            .Include(c => c.Books)
            .ToListAsync();

        return Ok(bookshelf);
    }

    // connect user to friends, for the created bookself
}
