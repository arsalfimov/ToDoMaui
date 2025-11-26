using MediatR;
using TDM.Api.Contracts.Users;

namespace TDM.Server.Application.Features.Users.Queries;

public record GetUsersByRoleIdQuery(long RoleId) : IRequest<IReadOnlyCollection<UserResponse>>;
