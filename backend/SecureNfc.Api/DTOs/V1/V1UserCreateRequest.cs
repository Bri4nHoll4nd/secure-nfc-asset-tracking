namespace SecureNfc.Api.DTOs;

public class V1UserCreateRequest
{
    public string Name { get; set; } = string.Empty;

    public string Status { get; set; } = "Active";
}