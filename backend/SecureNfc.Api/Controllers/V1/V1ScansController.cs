using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SecureNfc.Api.DTOs;
using SecureNfc.Api.Hubs;
using SecureNfc.Api.Services;
using SecureNfc.Data;

namespace SecureNfc.Api.Controllers.V1;

[ApiController]
[Route("api/1.0/[controller]")]
public class V1ScansController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly LatestScanService _latestScanService;
    private readonly IHubContext<ScanHub> _scanHub;

    public V1ScansController(AppDbContext dbContext, LatestScanService latestScanService, IHubContext<ScanHub> scanHub)
    {
        _dbContext = dbContext;
        _latestScanService = latestScanService;
        _scanHub = scanHub;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(V1ScanResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<V1ScanResponse>> Scan(V1TagUidScanRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.TagUid))
        {
            return BadRequest("Tag UID is required.");
        }

        string uid = NormalizeUid(request.TagUid);

        if (string.IsNullOrWhiteSpace(uid))
        {
            return BadRequest("Tag UID is required.");
        }

        if (uid.Any(character => !Uri.IsHexDigit(character)))
        {
            return BadRequest(
                "Tag UID must contain only hexadecimal characters.");
        }

        var tag = await _dbContext.Tags
            .AsNoTracking()
            .Include(tag => tag.Asset)
            .Include(tag => tag.User)
            .SingleOrDefaultAsync(tag => tag.Uid == uid);

        var scan = new V1ScanResponse
        {
            TagUid = uid,
            Source = string.IsNullOrWhiteSpace(request.Source)
                ? "Unknown"
                : request.Source,
            ScannedAtUtc = DateTime.UtcNow,
            Registered = tag is not null,
            TagId = tag?.Id
        };

        if (tag?.Asset is not null)
        {
            scan.EntityType = "Asset";
            scan.EntityId = tag.Asset.Id;
            scan.Name = tag.Asset.Name;
            scan.Status = tag.Asset.Status;
        }
        else if (tag?.User  is not null)
        {
            scan.EntityType = "User";
            scan.EntityId = tag.User.Id;
            scan.Name = tag.User.Name;
            scan.Status = tag.User.Status;
        }
        else if (tag is not null)
        {
            scan.EntityType = "Unassigned";
        }

        _latestScanService.SetLatest(scan);

        await _scanHub.Clients.All.SendAsync(
            "ScanReceived",
            scan);

        Console.WriteLine(
            $"NFC scan: {scan.TagUid} " +
            $"from {scan.Source} - " +
            $"{scan.EntityType}");

        return Ok(scan);
    }

    [HttpGet("latest")]
    [ProducesResponseType(
        typeof(V1ScanResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    public ActionResult<V1ScanResponse> GetLatest()
    {
        var scan = _latestScanService.GetLatest();

        if (scan is null)
        {
            return NoContent();
        }

        return Ok(scan);
    }

    private static string NormalizeUid(string uid)
    {
        return uid
            .Replace(" ", "")
            .Replace(":", "")
            .Replace("-", "")
            .ToUpperInvariant();
    }
}