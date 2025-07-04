using TripManager.Common.Primitives.Domain;

namespace TripManager.Domain.Users.ValueObjects;

public sealed record Username : ValueObject
{
    public string Value { get; set; }

    public Username(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length is > 30 or < 3)
        {
            throw new ArgumentException("Username cannot be empty", nameof(value));
        }

        Value = value;
    }

    public static implicit operator Username(string value) => new(value);
    public static implicit operator string(Username username) => username.Value;

    public override string ToString() => Value;
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}