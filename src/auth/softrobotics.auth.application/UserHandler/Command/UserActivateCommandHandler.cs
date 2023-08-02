using MediatR;
using Microsoft.EntityFrameworkCore;
using softrobotics.auth.application.Common.Interface;
using softrobotics.auth.application.Common.Model;
using softrobotics.auth.domain.Entity;

namespace softrobotics.auth.application.UserHandler.Command
{
    public record UserActivateCommand : IRequest<Result>
    {
        public int UserID { get; set; }
        public string UUID { get; set; }
    }

    public class UserActivateCommandHandler : IRequestHandler<UserActivateCommand, Result>
    {
        private readonly ISoftRoboticsDbContext context;

        public UserActivateCommandHandler(ISoftRoboticsDbContext context)
        {
            this.context = context;
        }

        public async Task<Result> Handle(UserActivateCommand request, CancellationToken cancellationToken)
        {
            List<string> errors = new();
            UserValidate entity = await context.UserValidates
                                                 .Include(x => x.User)
                                                 .OrderByDescending(x => x.UserValidateId)
                                                 .FirstOrDefaultAsync(x =>
                                                        x.UserId == request.UserID &&
                                                        x.HashUUID == request.UUID &&
                                                        x.IsActive &&
                                                        x.Created.AddMinutes(10) >= DateTime.Now,
                                                 cancellationToken);

            if (entity == null) errors.Add("The url can not found.");
            if (entity != null && entity.User == null) errors.Add("The user can not found.");
            if (errors.Count > 0) return Result.Failure();

            entity!.IsActive = false;
            entity!.LastModified = DateTime.Now;
            context.UserValidates.Entry(entity).State = EntityState.Modified;

            User userEntity = context.Users.Entry(entity.User).Entity;
            userEntity.IsActive = true;
            userEntity.LastModified = DateTime.Now;
            context.Users.Entry(userEntity).State = EntityState.Modified;

            await context.SaveToDbAsync(cancellationToken);
            return Result.Success();
        }
    }
}