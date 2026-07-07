using Microsoft.AspNetCore.Mvc;
using SecureNfc.Api.Models.V1;

namespace SecureNfc.Api.Controllers.V1
{
    [ApiController]
    [Route("api/1.0/[controller]")]
    public class V1AssetsController : ControllerBase
    {
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var assets = new List<V1Asset>
            {
                new V1Asset { Id = "A01-23", Name = "Laptop", Status = "Active", MaintenanceStatus = "Not needed", CurrentHolderId = "AP2-34"},
                new V1Asset { Id = "A01-24", Name = "Projector", Status = "Active", MaintenanceStatus = "Not needed", CurrentHolderId = "AP2-35"}
            };

            return Ok(assets);
        }

        //[HttpGet("{tagUid}")]
        //public IActionResult GetAsset(string tagUid)
        //{

        //}

        //[HttpPost("Create")]
        //public
    }
}