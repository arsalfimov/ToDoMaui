namespace TDM.Api.Contracts.Users;

public record UserResponse(
    long Id,
    string Login,
    bool IsBlocked,
    long RoleId,
    string RoleName,
    string? Description,
    long CreatedAt,
    long? UpdatedAt);



