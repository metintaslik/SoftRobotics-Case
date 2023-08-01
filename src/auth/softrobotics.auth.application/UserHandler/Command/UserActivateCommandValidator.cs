using FluentValidation;

namespace softrobotics.auth.application.UserHandler.Command
{
    public class UserActivateCommandValidator : AbstractValidator<UserActivateCommand>
    {
        public UserActivateCommandValidator()
        {
            RuleFor(x => x).NotEmpty().NotNull();
            RuleFor(x => x.UserID).NotEmpty().Must(x => x > 0);
            RuleFor(x => x.UUID).NotEmpty().NotNull().MinimumLength(64).MaximumLength(64);
        }
    }
}