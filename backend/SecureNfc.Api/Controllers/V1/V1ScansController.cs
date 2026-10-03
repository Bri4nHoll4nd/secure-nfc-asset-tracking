using Microsoft.AspNetCore.Mvc;
using SecureNfc.Api.DTOs;
using SecureNfc.Api.Services;

namespace SecureNfc.Api.Controllers.V1;

[ApiController]
[Route("api/1.0/[controller]")]
public class V1ScansController : ControllerBase
{
    private readonly LatestScanService _latestScanService;

    public V1ScansController(LatestScanService latestScanService)
    {
        _latestScanService = latestScanService;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(V1ScanResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    public ActionResult<V1ScanResponse> Scan(V1TagUidScanRequest request)
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

        if (uid.Any(character =>
            !Uri.IsHexDigit(character)))
        {
            return BadRequest(
                "Tag UID must contain only hexadecimal characters.");
        }

        var scan = new V1ScanResponse
        {
            TagUid = uid,
            Source = string.IsNullOrWhiteSpace(request.Source)
                ? "Unknown"
                : request.Source,
            ScannedAtUtc = DateTime.UtcNow,
        };

        _latestScanService.SetLatest(scan);

        Console.WriteLine(
            $"NFC scan: {scan.TagUid} from {scan.Source}");

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