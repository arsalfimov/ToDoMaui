using MediatR;

namespace TDM.Server.Application.Features.Users.Commands;

public record DeleteRangeUserCommand(IReadOnlyCollection<long> Ids) : IRequest<int>;
