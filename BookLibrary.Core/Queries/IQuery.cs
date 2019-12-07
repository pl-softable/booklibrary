namespace BookLibrary.Core.Queries
{
    using MediatR;

    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}