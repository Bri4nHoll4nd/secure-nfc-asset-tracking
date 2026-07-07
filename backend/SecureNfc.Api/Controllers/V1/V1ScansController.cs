using Microsoft.AspNetCore.Mvc;
using SecureNfc.Api.DTOs;

namespace SecureNfc.Api.Controllers.V1
{
    [ApiController]
    [Route("api/1.0/[controller]")]
    public class V1ScansController : ControllerBase
    {
        [HttpPost]
        public IActionResult ReceiveScan(V1ScanRequest request)
        {
            Console.WriteLine("Scan received: ");
            Console.WriteLine($"Tag UID: {request.TagUid}");
            Console.WriteLine($"Tag ID: {request.TagId}");
            Console.WriteLine($"Tag Type: {request.TagType}");
            Console.WriteLine($"Tag Version: {request.TagVersion}");
            Console.WriteLine($"Tag Signature: {request.TagSignature}");

            return Ok(new
            {
                message = "Scan received",
                tagUid = request.TagUid,
                tagId = request.TagId,
                tagType = request.TagType,
                tagVersion = request.TagVersion,
                tagSignature = request.TagSignature
            });
        }
    }
}