using SecureNfc.Data;
using SecureNfc.Data.Models.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureNfc.Api.DTOs;

namespace SecureNfc.Api.Controllers.V1;

[ApiController]
[Route("api/1.0/[controller]")]
public class V1UsersController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public V1UsersController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<V1User>>> GetAll()
    {
        var users = await _dbContext.Users
            .AsNoTracking()
            .OrderBy(user => user.Id)
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<V1User>> GetById(int id)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user  => user.Id == id);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<V1User>> Create(
        V1UserCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("User name is required.");
        }

        var user = new V1User
        {
            Name = request.Name.Trim(),
            Status = request.Status.Trim()
        };

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = user.Id },
            user);
    }
}