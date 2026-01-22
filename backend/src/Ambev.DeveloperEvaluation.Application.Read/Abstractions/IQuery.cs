using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Read.Abstractions;

public interface IQuery<TResult> : IRequest<TResult>
{
    
}