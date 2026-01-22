using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;

/// <summary>
/// API response model for CreateUser operation
/// </summary>
public class UpdateUserResponse
{
    /// <summary>
    /// The unique identifier of the created user
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The user's full name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The user's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
