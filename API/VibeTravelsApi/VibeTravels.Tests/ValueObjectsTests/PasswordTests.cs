using VibeTravels.Core.Exceptions.Password;
using VibeTravels.Core.ValueObjects.User;

namespace VibeTravels.Tests.ValueObjectsTests;

[TestFixture]
public class PasswordTests
{
    [Test]
    public void Password_ShouldCreateSuccessfully_WhenValueIsValid()
    {
        var validPassword = "Password123";

        var password = new Password(validPassword);

        Assert.That(password, Is.Not.Null);
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void Password_ShouldThrowEmptyPasswordException_WhenValueIsNullOrWhitespace(string? invalidPassword)
    {
        Assert.Throws<EmptyPasswordException>(() => new Password(invalidPassword));
    }

    [Test]
    [TestCase("Short1")]
    [TestCase("1234567")]
    public void Password_ShouldThrowInvalidPasswordRangeException_WhenValueIsTooShort(string shortPassword)
    {
        Assert.Throws<InvalidPasswordRangeException>(() => new Password(shortPassword));
    }

    [Test]
    public void Password_ShouldThrowInvalidPasswordRangeException_WhenValueIsTooLong()
    {
        var tooLongPassword = new string('A', 50) + new string('1', 52);

        Assert.Throws<InvalidPasswordRangeException>(() => new Password(tooLongPassword));
    }

    [Test]
    [TestCase("password123")]
    [TestCase("alllowercase1")]
    public void Password_ShouldThrowWeakPasswordException_WhenNoUppercaseCharacter(string weakPassword)
    {
        Assert.Throws<WeakPasswordException>(() => new Password(weakPassword));
    }

    [Test]
    [TestCase("PASSWORD")]
    [TestCase("ALLUPPERCASE")]
    public void Password_ShouldThrowWeakPasswordException_WhenNoDigit(string weakPassword)
    {
        Assert.Throws<WeakPasswordException>(() => new Password(weakPassword));
    }

    [Test]
    public void Password_ShouldImplicitlyConvertToString()
    {
        var password = new Password("Password123");

        string passwordString = password;

        Assert.That(passwordString, Is.EqualTo("Password123"));
    }

    [Test]
    public void Password_ShouldImplicitlyConvertFromString()
    {
        string passwordString = "Password123";

        Password password = passwordString;

        Assert.That(password, Is.Not.Null);
    }
}

