namespace Postgres.Api.Models;

public class Tag 
{
    public int Id { get; set; }
    public required string Uid { get; set; }
    public required string EntityCode { get; set; }
    public required string Version { get; set; }
    public required List<byte> Signature { get; set; } = [];
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}