namespace SecureNfc.Data.Models.V1;

public class V1Tag 
{
    public int Id { get; set; }
    public required string Uid { get; set; }
    public required string Version { get; set; }
    public required List<byte> Signature { get; set; } = [];
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public int? AssetId { get; set; }
    public V1Asset? Asset { get; set; }

    public int? UserId { get; set; }
    public V1User? User { get; set; }
}