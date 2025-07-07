using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Database.Repositories;
using ToDoApp.Domain.User;

namespace ToDoApp.Infrastructure.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ToDoDbContext _context;

    public UserRepository(ToDoDbContext context)
    {
        _context = context;
    }

    // Sprawdza, czy użytkownik istnieje na podstawie adresu e-mail
    public async Task<bool> ExistsWithEmailAsync(Email email, CancellationToken cancellationToken = default)
        => await _context.Users.AnyAsync(x => x.Email == email, cancellationToken);

    // Sprawdza, czy użytkownik istnieje na podstawie nazwy użytkownika
    public Task<bool> ExistsWithUsernameAsync(Username username, CancellationToken cancellationToken = default)
        => _context.Users.AnyAsync(x => x.Username == username, cancellationToken);

    // Pobiera użytkownika na podstawie identyfikatora
    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
        => await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    // Pobiera użytkownika na podstawie adresu e-mail
    public async Task<User?> GetByEmailAsync(string email)
        => await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email);

    // dodaje nowego użytkownika do repozytorium
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    // usuwa użytkownika z repozytorium
    public void DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
    }
}