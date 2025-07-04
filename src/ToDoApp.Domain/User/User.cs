using ToDoApp.Common.Primitives.Domain;

namespace ToDoApp.Domain.User;

public class User : Entity<UserId>
{
    public UserId UserId { get; private set; }
    public Username Username { get; private set; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }

    private User()
    {
    }

    private User(UserId id, Username username, Email email, Password password) : base(id)
    {
        Username = username;
        Email = email;
        Password = password;
    }

    public static User Create(Username username, Email email, Password password)
        => new User(UserId.New(), username, email, password);
}