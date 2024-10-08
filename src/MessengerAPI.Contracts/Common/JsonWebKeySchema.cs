namespace MessengerAPI.Contracts.Common;

/// <summary>
/// Json web key schema
/// </summary>
public class JsonWebKeySchema
{
    public string Kty { get; set; } = null;

    public string Use { get; set; } = null;

    public string Kid { get; set; } = null;

    public string Alg { get; set; } = null;

    public string N { get; set; } = null; // RSA Modulus

    public string E { get; set; } = null; // RSA Exponent
}
