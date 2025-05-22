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
public class DisplayBooksController : ControllerBase
{
    private readonly AppDbContext _context;

    public DisplayBooksController(AppDbContext context)
    {
        _context = context;
    }

    // Everyone can get book list
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var courses = await _context.Books.ToListAsync();
        return Ok(courses);
    }

    // Everyone can see a specific book
    [HttpGet("{isbn}")]
    public async Task<ActionResult<Book>> GetBook(string isbn)
    {
        var book = await _context.Books.FirstOrDefaultAsync(i => i.ISBN == isbn);

        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }


}
