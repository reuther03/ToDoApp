using System.Diagnostics.CodeAnalysis;
using TripManager.Domain.Users;
using TripManager.Domain.Users.ValueObjects;

namespace TripManager.Application.Abstractions;

public interface IUserContext
{
    [MemberNotNullWhen(true, nameof(UserId), nameof(Email), nameof(Username))]
    public bool IsAuthenticated { get; }

    public UserId? UserId { get; }
    public Email? Email { get; }
    public Username? Username { get; }
}