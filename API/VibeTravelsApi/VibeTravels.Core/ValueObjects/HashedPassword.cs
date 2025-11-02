using VibeTravels.Core.Exceptions.Password;

namespace VibeTravels.Core.ValueObjects;

public sealed record HashedPassword
{
    public string Value { get; }
    
    public HashedPassword(string hashedValue)
    {
        if (string.IsNullOrWhiteSpace(hashedValue))
            throw new EmptyPasswordException();
        
        Value = hashedValue;
    }
    
    public static implicit operator string(HashedPassword password) => password.Value;
    public static implicit operator HashedPassword(string hashedValue) => new(hashedValue);
}