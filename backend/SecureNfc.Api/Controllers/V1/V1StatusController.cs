using Microsoft.AspNetCore.Mvc;

namespace SecureNfc.Api.Controllers.V1;

[ApiController]
[Route("api/1.0/[controller]")]
public class V1StatusController : ControllerBase 
{
    [HttpGet]
    public ActionResult<string> GetStatus() 
    {
        Console.WriteLine("Request from frontend recived");

        return Ok("Secure NFC API is running");
    }
}