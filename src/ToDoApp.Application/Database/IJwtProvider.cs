using TripManager.Domain.Users;

namespace TripManager.Application.Abstractions;

public interface IJwtProvider
{
    string Generate(User user);
}