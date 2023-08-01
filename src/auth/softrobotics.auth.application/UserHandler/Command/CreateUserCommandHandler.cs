using MediatR;
using softrobotics.auth.application.Common.Interface;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using softrobotics.auth.domain.Entity;
using softrobotics.shared.Common;
using softrobotics.auth.application.Common.Model;

namespace softrobotics.auth.application.UserHandler.Command;

public class CreateUserCommand : IRequest<Result>
{
    private string _password;

    public string Username { get; set; }
    public string Password { get { return _password; } set { _password = value.EncodeSHA256(); } }
    public string Mail { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
{
    private readonly ISoftRoboticsDbContext context;
    private readonly IMapper mapper;

    public CreateUserCommandHandler(ISoftRoboticsDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        bool isExists = await context.Users.AnyAsync(x => x.Username == request.Username || x.Mail == request.Mail, cancellationToken);
        if (isExists) return Result.Failure(new string[] { "The username or e-mail has been used before." });

        User entity = mapper.Map<User>(request,
                                opt => opt.AfterMap((obj, user) => user.Created = DateTime.Now));

        await context.Users.AddAsync(entity, cancellationToken);
        await context.SaveToDbAsync(cancellationToken);

        await context.UserValidates.AddAsync(
            new UserValidate
            {
                IsActive = true,
                UserId = entity.UserId,
                HashUUID = Guid.NewGuid().ToString().EncodeSHA256(),
                Created = DateTime.Now,
            }, cancellationToken);
        await context.SaveToDbAsync(cancellationToken);

        return Result.Success();
    }
}