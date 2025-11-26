using MediatR;

namespace TDM.Server.Application.Features.Users.Commands;

public record DeleteUserCommand(long Id) : IRequest;
