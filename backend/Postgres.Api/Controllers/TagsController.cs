using Postgres.Api.Data;
using Postgres.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Postgres.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public TagsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<Tag>>> GetAll()
    {
        var tags = await _dbContext.Tags
            .AsNoTracking()
            .OrderBy(tag => tag.Id)
            .ToListAsync();

        return Ok(tags);
    }


}