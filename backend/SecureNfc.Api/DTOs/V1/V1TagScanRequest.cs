namespace SecureNfc.Api.DTOs;

public class V1TagScanRequest
{
    public required string TagUid { get; set; }
    public required string TagId { get; set; }
    public required string TagVersion { get; set; }

    //A HMAC of the other tag data
    public required List<byte> TagSignature { get; set; } = [];
}