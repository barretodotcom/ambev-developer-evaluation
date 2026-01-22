using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;

/// <summary>
/// Represents a request to create a new user in the system.
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// Gets or sets the username. Must be unique and contain only valid characters.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address. Must be a valid email format.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}