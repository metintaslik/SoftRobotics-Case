using MediatR;
using Microsoft.EntityFrameworkCore;
using softrobotics.auth.application.Common.Interface;
using softrobotics.auth.application.Common.Model;
using softrobotics.auth.domain.Entity;
using softrobotics.shared.Common.Helpers;

namespace softrobotics.auth.application.UserHandler.Queries
{
    public class GetTokenQuery : IRequest<Result<TokenDto>>
    {
        private string _password;

        public string Username { get; set; }
        public string Password { get { return _password; } set { _password = value.EncodeSHA256(); } }
    }

    public class GetTokenQueryHandler : IRequestHandler<GetTokenQuery, Result<TokenDto>>
    {
        private readonly ISoftRoboticsDbContext context;
        private readonly ITokenHelper tokenHelper;

        public GetTokenQueryHandler(ISoftRoboticsDbContext context, ITokenHelper tokenHelper)
        {
            this.context = context;
            this.tokenHelper = tokenHelper;
        }

        public async Task<Result<TokenDto>> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
            User entity = await context.Users.FirstOrDefaultAsync(x => x.Username == request.Username && x.Password == request.Password, cancellationToken);
            if (entity == null) return Result<TokenDto>.Failure(new string[] { "The username or password incorrect, try again." });
            if (!entity.IsActive) return Result<TokenDto>.Failure(new string[] { "The user is deactivated, confirm via e-mail." });

            return Result<TokenDto>.Success(tokenHelper.CreateToken(entity));
        }
    }
}