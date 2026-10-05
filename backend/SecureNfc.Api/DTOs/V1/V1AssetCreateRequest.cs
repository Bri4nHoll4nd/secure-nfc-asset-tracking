namespace SecureNfc.Api.DTOs;

public class V1AssetCreateRequest
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = "Available";

    public string MaintenanceStatus { get; set; } = "None";

    public int? UserId { get; set; }
}