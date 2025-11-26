using MediatR;
using TDM.Api.Contracts.Users;

namespace TDM.Server.Application.Features.Users.Commands;

public record UpdateUserCommand(
    long Id,
    string Login,
    bool IsBlocked,
    long RoleId,
    string? Description = null) : IRequest<UserResponse>;
