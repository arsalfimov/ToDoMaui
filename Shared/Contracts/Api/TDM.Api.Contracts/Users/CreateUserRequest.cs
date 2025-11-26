namespace TDM.Api.Contracts.Users;

public record CreateUserRequest(
    string Login,
    string Password,
    long RoleId,
    string? Description = null);
