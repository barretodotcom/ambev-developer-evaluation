namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Represents the response returned after successfully creating a new user.
/// </summary>
/// <remarks>
/// This response contains the unique identifier of the newly created user,
/// which can be used for subsequent operations or reference.
/// </remarks>
public class UpdateUserResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the updated user.
    /// </summary>
    /// <value>A GUID that uniquely identifies the updated user in the system.</value>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets the updated user's full name.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets the updated user's emai.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
