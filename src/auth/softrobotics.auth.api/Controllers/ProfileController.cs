using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using softrobotics.auth.application.Common.Model;
using softrobotics.auth.application.ProfileHandler.Command;

namespace softrobotics.auth.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProfileController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("UpdateProfile")]
        [Produces("application/json")]
        public async Task<Result> UpdateProfile(UserUpdateCommand command)
        {
            return await mediator.Send<Result>(command, new());
        }

        [HttpPatch("UpdatePassword")]
        [Produces("application/json")]
        public async Task<Result> UpdatePassword(UserUpdatePasswordCommand command)
        {
            return await mediator.Send<Result>(command, new());
        }

        [HttpPatch("UpdateUserInDeActive")]
        [Produces("application/json")]
        public async Task<Result> UpdateUserInDeActive()
        {
            return await mediator.Send<Result>(new UserDeActivateCommand(), new());
        }
    }
}