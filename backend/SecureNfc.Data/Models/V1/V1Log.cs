namespace SecureNfc.Data.Models.V1;

public class V1Log
{
    public int Id { get; set; }

    public int AssetId { get; set; }
    public V1Asset Asset { get; set; } = null;

    public LogType Type { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Description { get; set; }

    public int? UserId { get; set; }
    public V1User? User { get; set; }
}