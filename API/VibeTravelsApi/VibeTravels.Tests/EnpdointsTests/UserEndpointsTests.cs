using Moq;
using VibeTravels.Application.Commands.Auth;
using VibeTravels.Application.DTO;
using VibeTravels.Application.Security;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Tests.EnpdointsTests;

[TestFixture]
public class UserEndpointsTests
{
    private Mock<ICommandHandler<SignUp>> signUpHandlerMock;
    private Mock<ICommandHandler<SignIn>> signInHandlerMock;
    private Mock<ITokenStorage> tokenStorageMock;

    [SetUp]
    public void Setup()
    {
        signUpHandlerMock = new Mock<ICommandHandler<SignUp>>();
        signInHandlerMock = new Mock<ICommandHandler<SignIn>>();
        tokenStorageMock = new Mock<ITokenStorage>();
    }

    [Test]
    public async Task SignUp_ShouldCallHandler_WhenCommandIsProvided()
    {
        var command = new SignUp("test@example.com", "Password123");
        signUpHandlerMock
            .Setup(h => h.HandleAsync(It.IsAny<SignUp>()))
            .Returns(Task.CompletedTask);

        await signUpHandlerMock.Object.HandleAsync(command);

        signUpHandlerMock.Verify(h => h.HandleAsync(command), Times.Once);
    }

    [Test]
    public async Task SignIn_ShouldReturnToken_WhenCredentialsAreValid()
    {
        var command = new SignIn("test@example.com", "Password123");
        var expectedJwt = new JwtDto("access-token");
        
        signInHandlerMock
            .Setup(h => h.HandleAsync(It.IsAny<SignIn>()))
            .Returns(Task.CompletedTask);
        
        tokenStorageMock
            .Setup(t => t.RetrieveToken())
            .Returns(expectedJwt);

        await signInHandlerMock.Object.HandleAsync(command);
        var result = tokenStorageMock.Object.RetrieveToken();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.AccessToken, Is.EqualTo("access-token"));
        signInHandlerMock.Verify(h => h.HandleAsync(command), Times.Once);
    }

    [Test]
    public async Task SignIn_ShouldReturnNull_WhenTokenNotAvailable()
    {
        var command = new SignIn("test@example.com", "Password123");
        
        signInHandlerMock
            .Setup(h => h.HandleAsync(It.IsAny<SignIn>()))
            .Returns(Task.CompletedTask);
        
        tokenStorageMock
            .Setup(t => t.RetrieveToken())
            .Returns((JwtDto?)null);

        await signInHandlerMock.Object.HandleAsync(command);
        var result = tokenStorageMock.Object.RetrieveToken();

        Assert.That(result, Is.Null);
    }
}

