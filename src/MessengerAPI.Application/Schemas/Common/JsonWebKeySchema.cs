namespace MessengerAPI.Application.Schemas.Common;

public class JsonWebKeySchema
{
    public string Kty { get; set; }

    public string Use { get; set; }

    public string Kid { get; set; }

    public string Alg { get; set; }

    public string N { get; set; }  // RSA Modulus

    public string E { get; set; }  // RSA Exponent
}
