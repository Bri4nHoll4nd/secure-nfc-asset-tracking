namespace SecureNfc.Data.Models.V1;

public class V1Asset
{
    public int Id { get; set; }
    public required string EntityCode { get; set; }
    public required string Name { get; set; }
    public required string Status { get; set; }
    public string MaintenanceStatus { get; set; } = string.Empty;

    public V1Tag? Tag { get; set; }

    public List<V1Log> Logs { get; set; } = [];

    public int? UserId { get; set; }
    public V1User? User { get; set; }
}