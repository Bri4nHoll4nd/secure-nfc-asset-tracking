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
            .AsNoTracking() //Uses less memory because it doesnt need to keep track of changes when its just a get command
            .OrderBy(tag => tag.Id)
            .ToListAsync();

        return Ok(tags);
    }

    [HttpGet("{entityCode}")]
    public async Task<ActionResult<Tag>> GetByEntityCode(string entityCode)
    {
        var tag = await _dbContext.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(tag => tag.EntityCode == entityCode);

        if (tag is null)
        {
            return NotFound();
        }

        return Ok(tag);
    }

    [HttpPost]
    public async Task<ActionResult<Tag>> Create(Tag tag)
    {
        bool entityCodeExists = await _dbContext.Tags
            .AnyAsync(t => t.EntityCode == tag.EntityCode);

        if (entityCodeExists)
        {
            return Conflict("A tag with this EntityCode already exists.");
        }

        tag.CreatedAtUtc = DateTime.UtcNow;

        _dbContext.Tags.Add(tag);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetByEntityCode),
            new { EntityCode = tag.EntityCode },
            tag);
    }

    [HttpPut("{entityCode}")]
    public async Task<IActionResult> Update(string entityCode, Tag updatedTag)
    {
        var existingTag = await _dbContext.Tags.FirstOrDefaultAsync(tag => tag.EntityCode == entityCode);

        if (existingTag is null)
        {
            return NotFound();
        }

        existingTag.Uid = updatedTag.Uid;
        existingTag.Version = updatedTag.Version;
        existingTag.Signature = updatedTag.Signature;

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{entityCode}")]
    public async Task<IActionResult> Delete(string entityCode)
    {
        var tag = await _dbContext.Tags.FirstOrDefaultAsync(tag => tag.EntityCode == entityCode);

        if (tag is null)
        {
            return NotFound();
        }

        _dbContext.Tags.Remove(tag);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}