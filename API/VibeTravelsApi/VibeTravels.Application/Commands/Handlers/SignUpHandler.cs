using VibeTravels.Application.Exceptions;
using VibeTravels.Application.Exceptions.Auth;
using VibeTravels.Application.Security;
using VibeTravels.Application.Specifications.Users;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands.Handlers;

public sealed class SignUpHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher): ICommandHandler<SignUp>
{
    public async Task HandleAsync(SignUp command)
    {
        Email email = command.Email;
        Password password = command.Password;

        User? existingUser = await userRepository
            .GetUserOrDefault(new UserEmailSpecification(email));
        
        if (existingUser is not null)
            throw new UserAlreadyExistsException(email);
        
        HashedPassword hashedPassword = passwordHasher.Hash(password);
        
        User user = new(
            Guid.NewGuid(),
            email,
            hashedPassword,
            DateTime.UtcNow, 
            DateTime.UtcNow);

        await userRepository.AddUser(user);
    }
}