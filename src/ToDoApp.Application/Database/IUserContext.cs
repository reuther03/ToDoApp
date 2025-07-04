using System.Diagnostics.CodeAnalysis;
using ToDoApp.Domain.User;

namespace ToDoApp.Application.Database;

public interface IUserContext
{
    [MemberNotNullWhen(true, nameof(UserId), nameof(Email), nameof(Username))]
    public bool IsAuthenticated { get; }

    public UserId? UserId { get; }
    public Email? Email { get; }
    public Username? Username { get; }
}