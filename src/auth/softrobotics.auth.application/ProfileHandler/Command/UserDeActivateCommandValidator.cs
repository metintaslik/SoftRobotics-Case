using FluentValidation;

namespace softrobotics.auth.application.ProfileHandler.Command;

public class UserDeActivateCommandValidator : AbstractValidator<UserDeActivateCommand>
{
    public UserDeActivateCommandValidator()
    {
        RuleFor(x => x).NotEmpty().NotEmpty();
    }
}