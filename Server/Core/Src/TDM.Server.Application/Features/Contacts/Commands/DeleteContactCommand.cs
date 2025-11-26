using MediatR;

namespace TDM.Server.Application.Features.Contacts.Commands;

public record DeleteContactCommand(long Id) : IRequest<bool>;
