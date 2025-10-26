using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Domain.Entities;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    AppDbContext context,
    IConfiguration configuration)
    : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly AppDbContext _context = context;
    private readonly IConfiguration _configuration = configuration;

    [HttpPost("register-user")]
    public async Task<IActionResult> Register([FromBody] RegisterDto payload)
    {
        var userExists = await userManager.FindByEmailAsync(payload.Email);

        if (userExists is not null)
        {
            return BadRequest("User already exists");
        }
        
        var user = new ApplicationUser
        {
            UserName = payload.Email,
            Email = payload.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        
        var result = await userManager.CreateAsync(user, payload.Password);
        
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        
        return Created(nameof(Register), user);
    }
}