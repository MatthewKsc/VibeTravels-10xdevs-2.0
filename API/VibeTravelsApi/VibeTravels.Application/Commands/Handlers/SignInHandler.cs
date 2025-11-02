using VibeTravels.Application.DTO;
using VibeTravels.Application.Exceptions;
using VibeTravels.Application.Exceptions.Auth;
using VibeTravels.Application.Security;
using VibeTravels.Application.Specifications.Users;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands.Handlers;

public sealed class SignInHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider,
    ITokenStorage tokenStorage) : ICommandHandler<SignIn>
{
    public async Task HandleAsync(SignIn command)
    {
        Email email = command.Email;
        Password password = command.Password;
        
        User? user = await userRepository.GetUserOrDefault(new UserEmailSpecification(email));

        if (user is null)
            throw new UserNotFoundException(email);

        if (!passwordHasher.Verify(password, user.Password))
            throw new InvalidCredentialsException();
        
        JwtDto jwt = jwtProvider.GenerateToken(user.Id, user.Email);
        tokenStorage.StoreToken(jwt);
    }
}