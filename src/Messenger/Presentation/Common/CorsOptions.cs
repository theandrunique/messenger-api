namespace Messenger.Presentation.Common;

public class CorsPolicy
{
    public List<string> AllowedOrigins { get; init; } = new();
    public List<string> AllowedMethods { get; init; } = new();
    public List<string> AllowedHeaders { get; init; } = new();
    public List<string> ExposedHeaders { get; init; } = new();
    public int MaxAge { get; init; } = new();
}
