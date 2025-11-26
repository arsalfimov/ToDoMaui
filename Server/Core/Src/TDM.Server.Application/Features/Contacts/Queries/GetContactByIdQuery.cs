using MediatR;
using TDM.Api.Contracts.Contacts;

namespace TDM.Server.Application.Features.Contacts.Queries;

public record GetContactByIdQuery(long Id) : IRequest<ContactResponse?>;
