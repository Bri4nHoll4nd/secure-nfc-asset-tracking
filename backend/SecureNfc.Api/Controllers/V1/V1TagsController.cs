using SecureNfc.Data;
using SecureNfc.Data.Models.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureNfc.Api.DTOs;

namespace SecureNfc.Api.Controllers.V1;

[ApiController]
[Route("api/1.0/[controller]")]
public class V1TagsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public V1TagsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<V1Tag>>> GetAll()
    {
        var tags = await _dbContext.Tags
            .AsNoTracking()
            .OrderBy(tag => tag.Id)
            .ToListAsync();

        return Ok(tags);
    }

    [HttpGet("{uid}")]
    public async Task<ActionResult<V1Tag>> GetByUid(
        string uid)
    {
        uid = NormalizeUid(uid);

        var tag = await _dbContext.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(tag =>
                tag.Uid == uid);

        if (tag is null)
        {
            return NotFound();
        }

        return Ok(tag);
    }

    [HttpPost]
    public async Task<ActionResult<V1Tag>> Create(
        V1TagCreateRequest request)
    {
        string uid = NormalizeUid(request.Uid);

        if (string.IsNullOrWhiteSpace(uid))
        {
            return BadRequest("UID is required.");
        }

        if (uid.Any(character =>
            !Uri.IsHexDigit(character)))
        {
            return BadRequest(
                "UID must contain only hexadecimal characters.");
        }

        if (
            request.AssetId is not null &&
            request.UserId is not null
        )
        {
            return BadRequest(
                "A tag cannot belong to both an asset and a user.");
        }

        bool uidExists = await _dbContext.Tags
            .AnyAsync(tag => tag.Uid == uid);

        if (uidExists)
        {
            return Conflict(
                "A tag with this UID already exists.");
        }

        if (request.AssetId is not null)
        {
            bool assetExists =
                await _dbContext.Assets.AnyAsync(
                    asset =>
                        asset.Id == request.AssetId);

            if (!assetExists)
            {
                return BadRequest(
                    $"Asset {request.AssetId} does not exist.");
            }

            bool assetAlreadyHasTag =
                await _dbContext.Tags.AnyAsync(
                    tag =>
                        tag.AssetId == request.AssetId);

            if (assetAlreadyHasTag)
            {
                return Conflict(
                    "That asset already has an NFC tag.");
            }
        }

        if (request.UserId is not null)
        {
            bool userExists =
                await _dbContext.Users.AnyAsync(
                    user =>
                        user.Id == request.UserId);

            if (!userExists)
            {
                return BadRequest(
                    $"User {request.UserId} does not exist.");
            }

            bool userAlreadyHasTag =
                await _dbContext.Tags.AnyAsync(
                    tag =>
                        tag.UserId == request.UserId);

            if (userAlreadyHasTag)
            {
                return Conflict(
                    "That user already has an NFC tag.");
            }
        }

        var tag = new V1Tag
        {
            Uid = uid,

            Version = string.IsNullOrWhiteSpace(
                request.Version)
                    ? "1"
                    : request.Version,

            Signature =
                request.Signature is { Count: > 0 }
                    ? request.Signature
                    : Enumerable
                        .Repeat((byte)0, 16)
                        .ToList(),

            AssetId = request.AssetId,
            UserId = request.UserId,

            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Tags.Add(tag);

        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetByUid),
            new { uid = tag.Uid },
            tag);
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