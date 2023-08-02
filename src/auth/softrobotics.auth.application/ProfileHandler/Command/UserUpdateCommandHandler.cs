using MediatR;
using Microsoft.EntityFrameworkCore;
using softrobotics.auth.application.Common.Interface;
using softrobotics.auth.application.Common.Model;
using softrobotics.auth.domain.Entity;

namespace softrobotics.auth.application.ProfileHandler.Command;

public record UserUpdateCommand : IRequest<Result>
{
    public string Username { get; set; }
    public string Mail { get; set; }
}

public class UserUpdateCommandHandler : IRequestHandler<UserUpdateCommand, Result>
{
    private readonly ISoftRoboticsDbContext context;
    private readonly ITokenHelper tokenHelper;

    public UserUpdateCommandHandler(ISoftRoboticsDbContext context, ITokenHelper tokenHelper)
    {
        this.context = context;
        this.tokenHelper = tokenHelper;
    }

    public async Task<Result> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        int userId = tokenHelper.GetClaimUserId();

        User entity = await context.Users.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (entity == null) return Result.Failure(new string[] { "User can not be found." });

        entity.Username = request.Username;
        entity.Mail = request.Mail;

        context.Users.Entry(entity).State = EntityState.Modified;
        await context.SaveToDbAsync(cancellationToken);

        return Result.Success();
    }
}