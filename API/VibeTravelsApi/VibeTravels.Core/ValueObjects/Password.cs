using VibeTravels.Core.Exceptions.Password;

namespace VibeTravels.Core.ValueObjects;

public sealed record Password
{
    string Value { get; }
    
    public Password(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new EmptyPasswordException();
        
        if (value.Length is < 8 or > 100)
            throw new InvalidPasswordRangeException();

        if (!value.Any(char.IsDigit) || !value.Any(char.IsUpper))
            throw new WeakPasswordException();
        
        Value = value;
    }
    
    public static implicit operator string(Password password) => password.Value;
    public static implicit operator Password(string value) => new(value);
}