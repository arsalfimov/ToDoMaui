using MediatR;
using TDM.Api.Contracts.Contacts;

namespace TDM.Server.Application.Features.Contacts.Queries;

public record GetAllContactsQuery : IRequest<IReadOnlyCollection<ContactResponse>>;
