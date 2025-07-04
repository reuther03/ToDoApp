using ToDoApp.Domain.User;

namespace ToDoApp.Application.Database;

public interface IJwtProvider
{
    string Generate(User user);
}