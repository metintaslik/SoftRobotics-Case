using MediatR;
using Microsoft.EntityFrameworkCore;
using softrobotics.auth.application.Common.Interface;
using softrobotics.auth.application.Common.Model;
using softrobotics.auth.domain.Entity;
using softrobotics.shared.Common.Helpers;

namespace softrobotics.auth.application.ProfileHandler.Command;

public record UserUpdatePasswordCommand : IRequest<Result>
{
    private string _password;
    public string Password { get { return _password; } set { _password = value.EncodeSHA256(); } }

}

public class UserUpdatePasswordCommandHandler : IRequestHandler<UserUpdatePasswordCommand, Result>
{
    private readonly ISoftRoboticsDbContext context;
    private readonly ITokenHelper tokenHelper;

    public UserUpdatePasswordCommandHandler(ISoftRoboticsDbContext context, ITokenHelper tokenHelper)
    {
        this.context = context;
        this.tokenHelper = tokenHelper;
    }

    public async Task<Result> Handle(UserUpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        int userId = tokenHelper.GetClaimUserId();
        User entity = await context.Users.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (entity == null) return Result.Failure(new string[] { "User can not be found." });

        entity.Password = request.Password;
        context.Users.Entry(entity).State = EntityState.Modified;
        await context.SaveToDbAsync(cancellationToken);

        return Result.Success();
    }
}