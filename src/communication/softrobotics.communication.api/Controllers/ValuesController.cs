using MassTransit;
using Microsoft.AspNetCore.Mvc;
using softrobotics.shared.Common.Helpers;
using softrobotics.shared.Models;

namespace softrobotics.communication.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IPublishEndpoint publishEndpoint;

        public ValuesController(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        /// <summary>
        /// RabbitMQ MassTransit Consumer test
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            publishEndpoint.Publish<UserCreatedEventModel>(new UserCreatedEventModel(1, Guid.NewGuid().ToString().EncodeSHA256(), "test@softrobotics.com"), new());
            return Ok();
        }
    }
}