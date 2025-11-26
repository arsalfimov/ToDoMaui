using Riok.Mapperly.Abstractions;
using TDM.Api.Contracts.Users;
using TDM.Domain.Entities;
using TDM.Server.Application.Features.Users.Commands;

namespace TDM.Server.Application.Mappings;

[Mapper]
public static partial class UserMappings
{
    public static partial LoginUserCommand ToCommand(this LoginUserRequest request);
    public static partial CreateUserCommand ToCommand(this CreateUserRequest request);
    
    public static UpdateUserCommand ToCommand(this UpdateUserRequest request, long id)
        => new UpdateUserCommand(id, request.Login, request.IsBlocked, request.RoleId, request.Description);

    public static UserResponse ToResponse(this UserEntity entity)
        => new UserResponse(
            entity.Id,
            entity.Login,
            entity.IsBlocked,
            entity.RoleId,
            entity.Role.Name,
            entity.Description,
            entity.CreatedAt,
            entity.UpdatedAt);
    
    public static LoginUserResponse ToLoginResponse(this UserEntity entity)
        => new LoginUserResponse(
            entity.Id,
            entity.Login,
            entity.RoleId,
            entity.Role.Name,
            entity.IsBlocked);
}
