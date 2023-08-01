using FluentValidation;

namespace softrobotics.auth.application.UserHandler.Queries;

public class GetTokenQueryValidator : AbstractValidator<GetTokenQuery>
{
    public GetTokenQueryValidator()
    {
        RuleFor(x => x).NotEmpty().NotNull();
        RuleFor(x => x.Username).NotEmpty().NotNull().MaximumLength(50);
        RuleFor(x => x.Password).NotEmpty().NotNull().MinimumLength(64).MaximumLength(64);
    }
}