using MediatR;
using TDM.Api.Contracts.Contacts;

namespace TDM.Server.Application.Features.Contacts.Queries;

public record GetContactsByNameQuery(string Name) : IRequest<IReadOnlyCollection<ContactResponse>>;
