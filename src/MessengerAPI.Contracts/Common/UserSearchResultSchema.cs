using MessengerAPI.Domain.Users;

namespace MessengerAPI.Contracts.Common;

public record UserSearchResultSchema
{
    public string Id { get; init; }
    public string Username { get; init; }
    public string GlobalName { get; init; }
    public string Image { get; set; }

    private UserSearchResultSchema(UserIndexModel model)
    {
        Id = model.Id.ToString();
        Username = model.Username;
        GlobalName = model.GlobalName;
        Image = model.Image;
    }

    public static UserSearchResultSchema From(UserIndexModel model) => new(model);
}
