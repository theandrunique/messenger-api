using System.ComponentModel.DataAnnotations;

namespace Messenger.Options;

public class ElasticSearchOptions
{
    [Required]
    [Url]
    public string Uri { get; init; } = null!;
}
