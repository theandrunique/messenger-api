namespace Messenger.Contracts.Common;

/// <summary>
/// Json web key schema
/// </summary>
public record JsonWebKeySchema
{
    public string Kty { get; init; } = null!;
    public string Use { get; init; } = null!;
    public string Kid { get; init; } = null!;
    public string Alg { get; init; } = null!;
    public string N { get; init; } = null!; // RSA Modulus
    public string E { get; init; } = null!; // RSA Exponent
}
