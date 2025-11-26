namespace TDM.Api.Contracts.Users;

public record LoginUserResponse(
    long Id,
    string Login,
    long RoleId,
    string RoleName,
    bool IsBlocked);
