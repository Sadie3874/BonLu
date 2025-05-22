using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BonLu.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Win32;
using Microsoft.AspNetCore.RateLimiting;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpPost("/register")]
    [EnableRateLimiting("sliding")]
    public async Task<IActionResult> Register([FromBody] Register register)
    {
        // If role doesn't exist, create
        if (!await _roleManager.RoleExistsAsync(register.Role))
        {
            await _roleManager.CreateAsync(new IdentityRole(register.Role));
        }

        // Registers a new user with the specified email, password, and role.
        var user = new ApplicationUser { UserName = register.Email, Email = register.Email };
        var result = await _userManager.CreateAsync(user, register.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _userManager.AddToRoleAsync(user, register.Role);

        // Log user in after account creation
        await _signInManager.SignInAsync(user, isPersistent: false);

        return Ok(new { message = "Registration successful" });
    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] Login login)
    {
        var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
        if (!result.Succeeded)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        return Ok(new { message = "Login successful" });
    }

    [Authorize]
    [HttpPost("/logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { message = "Logged out successfully." });
    }
}