using VibeTravels.Core.Exceptions.Email;
using VibeTravels.Core.ValueObjects.User;

namespace VibeTravels.Tests.ValueObjectsTests;

[TestFixture]
public class EmailTests
{
    [Test]
    public void Email_ShouldCreateSuccessfully_WhenValueIsValid()
    {
        var validEmail = "test@example.com";

        var email = new Email(validEmail);

        Assert.That(email.Value, Is.EqualTo(validEmail));
    }

    [Test]
    public void Email_ShouldConvertToLowerCase_WhenCreated()
    {
        var mixedCaseEmail = "Test@Example.COM";

        var email = new Email(mixedCaseEmail);

        Assert.That(email.Value, Is.EqualTo("test@example.com"));
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void Email_ShouldThrowEmptyEmailException_WhenValueIsNullOrWhitespace(string? invalidEmail)
    {
        Assert.Throws<EmptyEmailException>(() => new Email(invalidEmail));
    }

    [Test]
    public void Email_ShouldThrowEmailTooLongException_WhenValueExceedsMaxLength()
    {
        var tooLongEmail = new string('a', 90) + "@example.com";

        Assert.Throws<EmailToLongException>(() => new Email(tooLongEmail));
    }

    [Test]
    [TestCase("invalid-email")]
    [TestCase("@example.com")]
    [TestCase("test@")]
    [TestCase("test@@example.com")]
    public void Email_ShouldThrowInvalidEmailFormatException_WhenFormatIsInvalid(string invalidEmail)
    {
        Assert.Throws<InvalidEmailFormatException>(() => new Email(invalidEmail));
    }

    [Test]
    public void Email_ShouldImplicitlyConvertToString()
    {
        var email = new Email("test@example.com");

        string emailString = email;

        Assert.That(emailString, Is.EqualTo("test@example.com"));
    }

    [Test]
    public void Email_ShouldImplicitlyConvertFromString()
    {
        string emailString = "test@example.com";

        Email email = emailString;

        Assert.That(email.Value, Is.EqualTo(emailString));
    }
}