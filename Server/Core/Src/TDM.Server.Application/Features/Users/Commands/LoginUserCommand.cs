using MediatR;
using TDM.Api.Contracts.Users;

namespace TDM.Server.Application.Features.Users.Commands;

public record LoginUserCommand(
    string Login,
    string Password) : IRequest<LoginUserResponse>;
