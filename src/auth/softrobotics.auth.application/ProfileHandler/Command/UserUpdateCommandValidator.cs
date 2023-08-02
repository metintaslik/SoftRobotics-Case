using FluentValidation;

namespace softrobotics.auth.application.ProfileHandler.Command;

public class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
{
    public UserUpdateCommandValidator()
    {
        RuleFor(x => x).NotEmpty().NotNull();
        RuleFor(x => x.Username).NotEmpty().NotNull().MaximumLength(50);
        RuleFor(x => x.Mail).NotEmpty().NotNull().MaximumLength(100).EmailAddress();
    }
}