namespace BookLibrary.Library.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Core.Commands;
    using Core.Commands.Library;
    using Core.Models;
    using Core.Queries;
    using Core.Queries.Library;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Bson;

    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly ICommandBus commandBus;
        private readonly IQueryBus queryBus;

        public LibraryController(ICommandBus commandBus, IQueryBus queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }

        [HttpPost("addBook")]
        public async Task<IActionResult> AddBook([FromBody] AddBookCommand command)
        {
            command.Id = Guid.NewGuid();

            await this.commandBus.Send(command);

            return CreatedAtAction(nameof(GetBookDetails), new {command.Id}, command);
        }

        [HttpPost("lendBook")]
        public async Task<IActionResult> LendBook([FromBody] LendBookCommand command)
        {
            await this.commandBus.Send(command);

            return this.Ok(command);
        }

        [HttpGet("bookDetails/{id}")]
        public async Task<IActionResult> GetBookDetails(Guid id)
        {
            var query = new GetBookDetailsQuery(id);

            var result = await this.queryBus.Send<GetBookDetailsQuery, BookDetails>(query);

            return Ok(result);
        }

        [HttpGet("personDetails/{id}")]
        public async Task<IActionResult> GetGetPersonDetails(Guid id)
        {
            var query = new GetPersonDetailsQuery
            {
                PersonId = id
            };

            var result = await this.queryBus.Send<GetPersonDetailsQuery, PersonBook>(query);

            return Ok(result);
        }

        [HttpDelete("bookDetails/{id}/quantity/{quantity}")]
        public async Task<IActionResult> DeleteBook(Guid id, int quantity)
        {
            var command = new RemoveBookCommand
            {
                BookId = id,
                Quantity = quantity
            };

            await this.commandBus.Send(command);

            return Ok(command);
        }
    }
}