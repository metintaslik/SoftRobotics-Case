using MediatR;
using Microsoft.EntityFrameworkCore;
using softrobotics.auth.application.Common.Interface;
using softrobotics.auth.application.Common.Model;
using softrobotics.auth.domain.Entity;

namespace softrobotics.auth.application.ProfileHandler.Command
{
    public record UserDeActivateCommand : IRequest<Result>
    {
        public bool IsActive { get; set; }
    }

    public class UserDeActivateCommandHandler : IRequestHandler<UserDeActivateCommand, Result>
    {
        private readonly ISoftRoboticsDbContext context;
        private readonly ITokenHelper tokenHelper;

        public UserDeActivateCommandHandler(ISoftRoboticsDbContext context, ITokenHelper tokenHelper)
        {
            this.context = context;
            this.tokenHelper = tokenHelper;
        }


        public async Task<Result> Handle(UserDeActivateCommand request, CancellationToken cancellationToken)
        {
            int userId = tokenHelper.GetClaimUserId();

            User entity = await context.Users.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            if (entity == null) return Result.Failure(new string[] { "User can not be found." });

            entity.IsActive = request.IsActive;

            context.Users.Entry(entity).State = EntityState.Modified;
            await context.SaveToDbAsync(cancellationToken);

            return Result.Success();
        }
    }
}