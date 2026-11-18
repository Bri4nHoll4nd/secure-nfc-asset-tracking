using System.Security.Cryptography;
using System.Text;

public class TagSigningService
{
    private readonly byte[] _secret;

    public TagSigningService(IConfiguration config)
    {
        string key = config["Nfc:SigningKey"]
            ?? throw new InvalidOperationException(
                "NFC signing key is missing."
            );

        _secret = Convert.FromBase64String(key);

        if (_secret.Length < 32)
            throw new InvalidOperationException(
                "NFC signing key must be at least 32 bytes."
            );
    }

    public byte[] Sign(string uid, string version)
    {
        string normalizedUid = uid
            .Replace(":", "")
            .Replace("-", "")
            .Replace(" ", "")
            .ToUpperInvariant();

        byte[] data = Encoding.UTF8.GetBytes(
            $"NFC-TAG|{normalizedUid}|{version}"
        );

        byte[] hash = HMACSHA256.HashData(_secret, data);

        return hash[..16];
    }
}