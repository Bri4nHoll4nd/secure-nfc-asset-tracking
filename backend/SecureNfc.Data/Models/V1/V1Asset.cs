namespace SecureNfc.Data.Models.V1;

public class V1Asset
{
    public string Id { get; set; }
    public required string Name { get; set; }
    public required string Status { get; set; }
    public string MaintenanceStatus { get; set; } = string.Empty;
    public required string EntityCode { get; set; }
    public V1Tag Tag { get; set; } = null;
}