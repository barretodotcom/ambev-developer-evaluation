namespace Ambev.DeveloperEvaluation.Domain.Events.Users;

public record UserUpdatedDomainEvent(
    Guid Id,
    string Username,
    string Email
    ) : IDomainEvent;