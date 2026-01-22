using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Read.Abstractions;

public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    
}