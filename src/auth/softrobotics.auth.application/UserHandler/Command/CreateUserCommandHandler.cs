using MediatR;
using softrobotics.auth.application.Common.Interface;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using softrobotics.auth.domain.Entity;
using softrobotics.auth.application.Common.Model;
using softrobotics.shared.Common.Helpers;
using softrobotics.auth.application.UserHandler.Events;

namespace softrobotics.auth.application.UserHandler.Command;

public record CreateUserCommand : IRequest<Result>
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
    private readonly IMediator mediator;

    public CreateUserCommandHandler(ISoftRoboticsDbContext context, IMapper mapper, IMediator mediator)
    {
        this.context = context;
        this.mapper = mapper;
        this.mediator = mediator;
    }

    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        bool isExists = await context.Users.AnyAsync(x => x.Username == request.Username || x.Mail == request.Mail, cancellationToken);
        if (isExists) return Result.Failure(new string[] { "The username or e-mail has been used before." });

        User entity = mapper.Map<User>(request,
                                opt => opt.AfterMap((obj, user) => user.Created = DateTime.Now));

        await context.Users.AddAsync(entity, cancellationToken);
        await context.SaveToDbAsync(cancellationToken);

        UserValidate userValidateEntity = new()
        {
            IsActive = true,
            UserId = entity.UserId,
            HashUUID = Guid.NewGuid().ToString().EncodeSHA256(),
            Created = DateTime.Now,
        };
        await context.UserValidates.AddAsync(userValidateEntity, cancellationToken);
        await context.SaveToDbAsync(cancellationToken);

        UserCreatedEvent userCreatedEvent = new(new shared.Models.UserCreatedEventModel(entity.UserId, userValidateEntity.HashUUID, entity.Mail));
        await mediator.Publish(userCreatedEvent, cancellationToken);

        return Result.Success();
    }
}