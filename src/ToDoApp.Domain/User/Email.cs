using System.Text.RegularExpressions;
using ToDoApp.Common.Exceptions.Domain;
using ToDoApp.Common.Primitives.Domain;

namespace ToDoApp.Domain.User;

// ValueObject reprezentujący adres e-mail użytkownika
public sealed partial record Email : ValueObject
{
    public string Value { get; }

    // Konstruktor przyjmujący wartość e-mail
    public Email(string value)
    {
        Validate(value);
        Value = value;
    }

    // walidacja adresu e-mail
    private static void Validate(string value)
    {
        if (!EmailRegex().IsMatch(value))
            throw new DomainException("Invalid email: {0}", value);
    }

    // konwersja implikowana z Email do string i odwrotnie
    public static implicit operator string(Email email) => email.Value;

    // konwersja implikowana z string do Email
    public static implicit operator Email(string email) => new(email);


    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    // Regular expression to validate email format
    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-z]{2,}$")]
    private static partial Regex EmailRegex();
}