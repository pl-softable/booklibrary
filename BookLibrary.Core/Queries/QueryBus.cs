namespace BookLibrary.Core.Queries
{
    using System.Threading.Tasks;
    using MediatR;

    public class QueryBus : IQueryBus
    {
        private readonly IMediator _mediator;

        public QueryBus(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public Task<TResponse> Send<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>
        {
            return this._mediator.Send(query);
        }
    }
}