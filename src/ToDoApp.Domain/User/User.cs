﻿using ToDoApp.Common.Primitives.Domain;

namespace ToDoApp.Domain.User;

public class User : Entity<UserId>
{
    // nazwaz uzytkownika, email i hasło
    public Username Username { get; private set; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }

    private User()
    {
    }

    // Prywatny konstruktor do tworzenia instancji użytkownika
    private User(UserId id, Username username, Email email, Password password) : base(id)
    {
        Username = username;
        Email = email;
        Password = password;
    }

    // Metoda statyczna do tworzenia nowego użytkownika
    public static User Create(Username username, Email email, Password password)
        => new User(UserId.New(), username, email, password);
}