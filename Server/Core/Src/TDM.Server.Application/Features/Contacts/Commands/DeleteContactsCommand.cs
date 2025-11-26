using MediatR;

namespace TDM.Server.Application.Features.Contacts.Commands;

public record DeleteContactsCommand(IReadOnlyCollection<long> Ids) : IRequest<int>;
