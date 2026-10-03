namespace SecureNfc.Api.DTOs;

public class V1ScanResponse
{
    public string TagUid { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public DateTime ScannedAtUtc { get; set; }
}