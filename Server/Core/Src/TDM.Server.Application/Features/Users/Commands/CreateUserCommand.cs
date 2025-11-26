using MediatR;
using TDM.Api.Contracts.Users;

namespace TDM.Server.Application.Features.Users.Commands;

public record CreateUserCommand(
    string Login,
    string Password,
    long RoleId,
    string? Description = null) : IRequest<UserResponse>;
