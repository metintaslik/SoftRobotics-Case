using FluentValidation;

namespace softrobotics.auth.application.UserHandler.Command
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x).NotEmpty().NotNull();
            RuleFor(x => x.Username).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.Password).NotEmpty().NotNull().MinimumLength(64).MaximumLength(64);
            RuleFor(x => x.Mail).NotEmpty().NotNull().MaximumLength(100).EmailAddress();
        }
    }
}