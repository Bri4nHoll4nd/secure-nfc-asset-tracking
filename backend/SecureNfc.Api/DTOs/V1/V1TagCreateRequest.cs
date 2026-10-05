namespace SecureNfc.Api.DTOs;

public class V1TagCreateRequest
{
    public string Uid { get; set; } = string.Empty;

    public string Version { get; set; } = "1";

    public List<byte>? Signature { get; set; }

    public int? AssetId { get; set; }

    public int? UserId { get; set; }
}