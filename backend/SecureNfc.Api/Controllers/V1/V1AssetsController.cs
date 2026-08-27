using SecureNfc.Data;
using SecureNfc.Data.Models.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SecureNfc.Api.Controllers.V1;

[ApiController]
[Route("api/1.0/[controller]")]
public class V1AssetsController : ControllerBase 
{
    private readonly AppDbContext _dbContext;

    public V1AssetsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<List<V1Asset>>> GetAll() 
    {
        var assets = await _dbContext.Assets
            .AsNoTracking()
            .OrderBy(assets => assets.Id)
            .ToListAsync();

        return Ok(assets);
    }

    [HttpGet("{entityCode}")]
    public async Task<ActionResult<V1Asset>> GetByEntityCode(string entityCode)
    {
        var asset = await _dbContext.Assets
            .AsNoTracking()
            .FirstOrDefaultAsync(asset => asset.EntityCode == entityCode);

        if (asset is null)
        {
            return NotFound();
        }

        return Ok(asset);
    }

    [HttpPost]
    [ProducesResponseType(typeof(V1Asset), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<V1Asset>> Create(V1Asset asset)
    {
        bool entityCodeExists = await _dbContext.Assets
            .AnyAsync(a => a.EntityCode == asset.EntityCode);

        if (entityCodeExists)
        {
            return Conflict("A tag with this EntityCode already exists.");
        }

        _dbContext.Add(asset);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetByEntityCode),
            new { EntityCode = asset.EntityCode },
            asset);
    }

    [HttpPut("{entityCode}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string entityCode, V1Asset updatedAsset)
    {
        var existingAsset = await _dbContext.Assets.FirstOrDefaultAsync(asset => asset.EntityCode == entityCode);

        if (existingAsset is null)
        {
            return NotFound();
        }

        existingAsset.Name = updatedAsset.Name;
        existingAsset.Status = updatedAsset.Status;
        existingAsset.MaintenanceStatus = updatedAsset.MaintenanceStatus;

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}