namespace MessengerAPI.Infrastructure.Auth.SigningKeys;

public class JsonWebKey
{
    public string KeyType { get; set; }

    public string Use { get; set; }

    public string KeyId { get; set; }

    public string Algorithm { get; set; }

    public string N { get; set; }  // RSA Modulus

    public string E { get; set; }  // RSA Exponent
}
