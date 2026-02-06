
namespace Domain.Participants.ValueObjects;

public sealed record Email
{
    public string Value { get; }
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email cannot be empty.", nameof(value));
        }

        var trimmed = value.Trim();

        if (!trimmed.Contains("@") || trimmed.StartsWith("@") || trimmed.EndsWith("@"))
        {
            throw new ArgumentException("Invalid email format.", nameof(value));
        }

        Value = trimmed.ToLowerInvariant();
    }

    public override string ToString() => Value;


}
    


