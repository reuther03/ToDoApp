using ToDoApp.Common.Primitives.Domain;

namespace ToDoApp.Domain.User;

// UserId reprezentuje unikalny identyfikator użytkownika w aplikacji ToDoApp.
public record UserId : ValueObject
{
    public Guid Value { get; }

    public UserId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidCastException("Value cannot be empty.");
        }

        Value = value;
    }

    // Metody statyczne do tworzenia nowych instancji UserId
    public static UserId New() => new(Guid.NewGuid());
    public static UserId From(Guid value) => new(value);
    public static UserId From(string value) => new(Guid.Parse(value));

    public static implicit operator Guid(UserId userId) => userId.Value;
    public static implicit operator UserId(Guid userId) => new(userId);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}