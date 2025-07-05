using TripManager.Domain.Users;

namespace TripManager.Application.Features.Users.Dto;

public sealed class AccessToken
{
    public string Token { get; init; } = null!;
    public Guid UserId { get; init; }
    public string Fullname { get; init; } = null!;

    public static AccessToken Create(User user, string token)
    {
        return new AccessToken
        {
            Token = token,
            UserId = user.Id,
            Fullname = user.Fullname
        };
    }
}