namespace SecureNfc.Api.DTOs;

public class V1ScanRequest
{
    public required string TagUid { get; set; }
    public required string TagId { get; set; }
    public required string TagType { get; set; }
    public required string TagVersion { get; set; }

    //A HMAC of the other tag data
    public required string TagSignature { get; set; }
}