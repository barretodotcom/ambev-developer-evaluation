using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Abstractions.Commands;

public interface ICommand<out TResult> : IRequest<TResult>
{
    
}

public interface ICommand : IRequest
{
    
}