namespace MessengerAPI.Application.Schemas.Common;

/// <summary>
/// Email schema for response
/// </summary>
public record EmailSchema
{
    /// <summary>
    /// Email
    /// </summary>
    public string Data { get; init; }
    /// <summary>
    /// Is email verified
    /// </summary>
    public bool IsVerified { get; init; }
    /// <summary>
    /// Is email public
    /// </summary>
    public bool IsPublic { get; init; }
    /// <summary>
    /// Date when email was added
    /// </summary>
    public DateTime AddedAt { get; init; }
}
