using Ambev.DeveloperEvaluation.Application.Abstractions.Commands;
using Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;
using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Common.Security;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Handler for processing UpdateUserCommand requests
/// </summary>
public class UpdateUserHandler : ICommandHandler<UpdateUserCommand, UpdateUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Initializes a new instance of UpdateUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="unitOfWork">The unitOfWork to control transactions</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="passwordHasher">The responsible for hash the user's password</param>
    public UpdateUserHandler(IUserRepository userRepository,  IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Handles the UpdateUserCommand request
    /// </summary>
    /// <param name="command">The UpdateUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created user details</returns>
    public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.Id, cancellationToken);
        
        if (user == null)
            throw new DomainException($"User does not exist");

        var existingUser = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        
        if (existingUser != null && existingUser.Id != command.Id)
            throw new DomainException($"User with email {command.Email} already exists");

        user.UpdateUsernameAndEmail(command.Username, command.Email);
        
         _userRepository.Update(user);
        
        await _unitOfWork.CommitAsync(cancellationToken);
        
        var result = _mapper.Map<UpdateUserResult>(existingUser);
        return result;
    }
}
