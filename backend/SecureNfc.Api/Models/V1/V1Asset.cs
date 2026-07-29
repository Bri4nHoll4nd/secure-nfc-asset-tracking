namespace SecureNfc.Api.Models.V1;

public class V1Asset
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public string MaintenanceStatus { get; set; }
    public string CurrentHolderId { get; set; }
}