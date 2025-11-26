using MediatR;
using TDM.Api.Contracts.Contacts;

namespace TDM.Server.Application.Features.Contacts.Commands;

public record UpdateContactCommand(
    long Id,
    string FirstName,
    string LastName,
    string? Phone = null,
    string? Email = null,
    string? Address = null,
    string? Description = null) : IRequest<ContactResponse>;
