using FluentValidation;
using Messenger.Data.Interfaces.Users;

namespace Messenger.Application.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private readonly IUserRepository _userRepository;
    public RegisterCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Username)
            .NotEmpty()
            .Length(3, 50)
            .Matches(@"^[a-zA-Z0-9]+$")
            .WithMessage("Username must contain only letters and numbers.")
            .MustAsync(async (value, _) =>
            {
                return !await _userRepository.IsExistsByUsernameAsync(value);
            })
            .WithMessage("Username already exists.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(8, 50)
            .Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character from #?!@$%^&*-.");

        RuleFor(x => x.GlobalName)
            .NotEmpty()
            .Length(3, 50)
            .Matches(@"^[a-zA-Z0-9]+$")
            .WithMessage("Global name must contain only letters and numbers.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (value, _) =>
            {
                return !await _userRepository.IsExistsByEmailAsync(value);
            })
            .WithMessage("Email is busy.");
    }
}
