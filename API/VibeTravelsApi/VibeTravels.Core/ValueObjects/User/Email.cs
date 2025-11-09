using System.Text.RegularExpressions;
using VibeTravels.Core.Exceptions.Email;

namespace VibeTravels.Core.ValueObjects.User;

public sealed record Email
{
    private readonly Regex validateEmailRegex = new("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
    private const int MaxEmailLength = 100;
    
    public string Value { get; }
    
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new EmptyEmailException();

        if (value.Length > MaxEmailLength)
            throw new EmailToLongException();
        
        value = value.ToLowerInvariant();

        if (!validateEmailRegex.IsMatch(value))
            throw new InvalidEmailFormatException();
        
        Value = value;
    }
    
    public static implicit operator string(Email email) => email.Value;
    public static implicit operator Email(string value) => new(value);
}