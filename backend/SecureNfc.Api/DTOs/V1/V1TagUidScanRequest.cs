namespace SecureNfc.Api.DTOs;

public class V1TagUidScanRequest 
{
    public string TagUid { get; set; } = string.Empty;
    public string Source { get; set; } = "Unknown";
}