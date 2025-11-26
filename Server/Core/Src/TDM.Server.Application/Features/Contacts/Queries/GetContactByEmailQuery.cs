using MediatR;
using TDM.Api.Contracts.Contacts;

namespace TDM.Server.Application.Features.Contacts.Queries;

public record GetContactByEmailQuery(string Email) : IRequest<ContactResponse?>;
