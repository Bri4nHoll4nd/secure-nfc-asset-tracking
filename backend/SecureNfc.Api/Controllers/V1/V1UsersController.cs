using SecureNfc.Data;
using SecureNfc.Data.Models.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SecureNfc.Api.Controllers.V1;

[ApiController]
[Route("api/[controller]")]
public class V1UsersController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public V1UsersController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}