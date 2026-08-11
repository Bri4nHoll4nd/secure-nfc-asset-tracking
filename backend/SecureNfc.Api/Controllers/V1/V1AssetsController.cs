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
}