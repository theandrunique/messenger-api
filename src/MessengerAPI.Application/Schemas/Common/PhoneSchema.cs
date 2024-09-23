namespace MessengerAPI.Application.Schemas.Common;

/// <summary>
/// Phone schema for response
/// </summary>
public record PhoneSchema
{
    /// <summary>
    /// Phone
    /// </summary>
    public string Data { get; init; } = null!;
    /// <summary>
    /// Is phone verified
    /// </summary>
    public bool IsVerified { get; init; }
    /// <summary>
    /// Date of adding
    /// </summary>
    public DateTime AddedAt { get; init; }
}
