using ToDoApp.Domain.User;

namespace ToDoApp.Application.Dto;

public sealed class AccessToken
{
    public string Token { get; init; } = null!;
    public Guid UserId { get; init; }
    public string Username { get; init; } = null!;

    public static AccessToken Create(User user, string token)
    {
        return new AccessToken
        {
            Token = token,
            UserId = user.Id,
            Username = user.Username
        };
    }
}