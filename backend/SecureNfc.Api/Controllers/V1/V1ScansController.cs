using Microsoft.AspNetCore.Mvc;
using SecureNfc.Api.DTOs;

namespace SecureNfc.Api.Controllers.V1;

[ApiController]
[Route("api/1.0/[controller]")]
public class V1ScansController : ControllerBase
{
    [HttpPost("ScanTag")]
    public IActionResult ScanData(V1TagScanRequest request)
    {
        Console.WriteLine("Scan received: ");
        Console.WriteLine($"Tag UID: {request.TagUid}");
        Console.WriteLine($"Tag ID: {request.TagId}");
        Console.WriteLine($"Tag Version: {request.TagVersion}");
        Console.WriteLine($"Tag Signature: {Convert.ToHexString(request.TagSignature.ToArray())}");

        return Ok(new
        {
            message = "Scan received",
            tagUid = request.TagUid,
            tagId = request.TagId,
            tagVersion = request.TagVersion,
            tagSignature = request.TagSignature
        });
    }

    [HttpPost("ScanTagUid")]
    public IActionResult ScanUid(V1TagUidScanRequest request)
    {
        Console.WriteLine($"Recived tag: {request.TagUid}");

        return Ok(new
        {
            message = "Scan received",
            tagUid = request.TagUid
        });
    }
}