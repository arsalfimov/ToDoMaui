namespace TDM.Api.Contracts.Users;
public record UpdateUserRequest(
    string Login,
    bool IsBlocked,
    long RoleId,
    string? Description = null);
