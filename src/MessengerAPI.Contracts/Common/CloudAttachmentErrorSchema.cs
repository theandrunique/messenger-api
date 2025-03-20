using System.Text.Json.Serialization;

namespace MessengerAPI.Contracts.Common;

public class CloudAttachmentErrorSchema
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Id { get; set; }
    public List<string> Errors { get; set; }

    public CloudAttachmentErrorSchema(string? id, List<string> errors)
    {
        Id = id;
        Errors = errors;
    }
}
