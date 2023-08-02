using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using softrobotics.auth.application.Common.Model;
using softrobotics.auth.application.UserHandler.Command;
using softrobotics.auth.application.UserHandler.Queries;
using softrobotics.shared.Models;

namespace softrobotics.auth.api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("GetToken")]
        [Produces("application/json")]
        public async Task<Result<TokenDto>> GetToken(GetTokenQuery query) =>
           await mediator.Send<Result<TokenDto>>(query);

        [HttpPost("Register")]
        [Produces("application/json")]
        public async Task<Result> Register(CreateUserCommand command) =>
            await mediator.Send<Result>(command, new());

        [HttpGet]
        [Produces("application/json")]
        [Route("[action]/{userid}/{uuid}")]
        public async Task<Result> GetActive(int userid, string uuid) =>
            await mediator.Send<Result>(new UserActivateCommand { UserID = userid, UUID = uuid }, new());
    }
}