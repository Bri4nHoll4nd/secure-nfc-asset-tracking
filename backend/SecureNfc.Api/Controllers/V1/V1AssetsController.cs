using SecureNfc.Data;
using SecureNfc.Data.Models.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureNfc.Api.DTOs;

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

    [HttpGet]
    public async Task<ActionResult<List<V1Asset>>> GetAll()
    {
        var assets = await _dbContext.Assets
            .AsNoTracking()
            .OrderBy(asset => asset.Id)
            .ToListAsync();

        return Ok(assets);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<V1Asset>> GetById(int id)
    {
        var asset = await _dbContext.Assets
            .AsNoTracking()
            .FirstOrDefaultAsync(asset => asset.Id == id);

        if (asset is null)
        {
            return NotFound();
        }

        return Ok(asset);
    }

    [HttpPost]
    public async Task<ActionResult<V1Asset>> Create(
        V1AssetCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Asset name is required.");
        }

        if (request.UserId is not null)
        {
            bool userExists = await _dbContext.Users
                .AnyAsync(user =>
                    user.Id == request.UserId);

            if (!userExists)
            {
                return BadRequest(
                    $"User {request.UserId} does not exist.");
            }
        }

        var asset = new V1Asset
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            Status = request.Status.Trim(),
            MaintenanceStatus =
                request.MaintenanceStatus.Trim(),

            UserId = request.UserId
        };

        _dbContext.Assets.Add(asset);

        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = asset.Id },
            asset);
    }
}