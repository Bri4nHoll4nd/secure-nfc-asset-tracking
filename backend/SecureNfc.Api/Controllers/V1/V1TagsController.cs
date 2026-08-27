using SecureNfc.Data;
using SecureNfc.Data.Models.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SecureNfc.Api.Controllers.V1;

[ApiController]
[Route("api/[controller]")]
public class V1TagsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public V1TagsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<V1Tag>>> GetAll()
    {
        var tags = await _dbContext.Tags
            .AsNoTracking() //Uses less memory because it doesnt need to keep track of changes when its just a get command
            .OrderBy(tag => tag.Id)
            .ToListAsync();

        return Ok(tags);
    }

    [HttpGet("{uid}")]
    public async Task<ActionResult<V1Tag>> GetByUid(string uid)
    {
        var tag = await _dbContext.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(tag => tag.Uid == uid);

        if (tag is null)
        {
            return NotFound();
        }

        return Ok(tag);
    }

    [HttpPost]
    [ProducesResponseType(typeof(V1Tag), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<V1Tag>> Create(V1Tag tag)
    {
        bool uidExists = await _dbContext.Tags
            .AnyAsync(t => t.Uid == tag.Uid);

        if (uidExists)
        {
            return Conflict("A tag with this Uid already exists.");
        }

        tag.CreatedAtUtc = DateTime.UtcNow;

        _dbContext.Tags.Add(tag);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetByUid),
            new { Uid = tag.Uid },
            tag);
    }

    [HttpPut("{uid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string uid, V1Tag updatedTag)
    {
        var existingTag = await _dbContext.Tags.FirstOrDefaultAsync(tag => tag.Uid == uid);

        if (existingTag is null)
        {
            return NotFound();
        }

        existingTag.Version = updatedTag.Version;
        existingTag.Signature = updatedTag.Signature;
        existingTag.AssetId = updatedTag.AssetId;

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{uid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string uid)
    {
        var tag = await _dbContext.Tags.FirstOrDefaultAsync(tag => tag.Uid == uid);

        if (tag is null)
        {
            return NotFound();
        }

        _dbContext.Tags.Remove(tag);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}