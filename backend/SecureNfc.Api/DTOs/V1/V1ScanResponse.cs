namespace SecureNfc.Api.DTOs;

public class V1ScanResponse
{
    public string TagUid { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public DateTime ScannedAtUtc { get; set; }
    public bool Registered { get; set; }
    public int? TagId { get; set; }
    public string EntityType { get; set; } = "Unknown";
    public int? EntityId { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }
}