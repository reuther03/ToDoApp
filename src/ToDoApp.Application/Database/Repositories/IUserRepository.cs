using ToDoApp.Domain.User;

namespace ToDoApp.Application.Database.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsWithEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<bool> ExistsWithUsernameAsync(Username username, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    void DeleteAsync(User user, CancellationToken cancellationToken = default);

}