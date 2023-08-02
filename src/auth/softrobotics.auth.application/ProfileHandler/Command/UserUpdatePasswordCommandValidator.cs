using FluentValidation;

namespace softrobotics.auth.application.ProfileHandler.Command;

public class UserUpdatePasswordCommandValidator : AbstractValidator<UserUpdatePasswordCommand>
{
    public UserUpdatePasswordCommandValidator()
    {
        RuleFor(x => x.Password).NotEmpty().NotNull().MinimumLength(64).MaximumLength(64);
    }
}