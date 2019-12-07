namespace BookLibrary.Identity.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Core.Commands;
    using Core.Commands.Identity;
    using Core.Models;
    using Core.Queries;
    using Core.Queries.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ICommandBus commandBus;
        private readonly IQueryBus queryBus;

        public UsersController(ICommandBus commandBus, IQueryBus queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetLibrarianToken([FromBody] GetTokenQuery query)
        {
            var result = await this.queryBus.Send<GetTokenQuery, TokenResponse>(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegisteredPerson([FromBody] AddPersonCommand command)
        {
            command.Id = Guid.NewGuid();

            await this.commandBus.Send(command);

            return CreatedAtAction(nameof(GetRegisteredPerson), new {command.Id}, command);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegisteredPerson(Guid id)
        {
            var query = new GetPersonQuery
            {
                Id = id
            };

            var result = await this.queryBus.Send<GetPersonQuery, Person>(query);

            return Ok(result);
        }
    }
}