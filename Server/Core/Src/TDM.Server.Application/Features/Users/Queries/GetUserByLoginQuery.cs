using MediatR;
using TDM.Api.Contracts.Users;

namespace TDM.Server.Application.Features.Users.Queries;

public record GetUserByLoginQuery(string Login) : IRequest<UserResponse>;
