using Riok.Mapperly.Abstractions;
using TDM.Api.Contracts.Contacts;
using TDM.Domain.Entities;
using TDM.Server.Application.Features.Contacts.Commands;

namespace TDM.Server.Application.Mappings;

[Mapper]
public static partial class ContactMappings
{
    public static partial CreateContactCommand ToCommand(this CreateContactRequest request);
    
    public static UpdateContactCommand ToCommand(this UpdateContactRequest request, long id)
        => new UpdateContactCommand(id, request.FirstName, request.LastName, request.Phone, request.Email, request.Address, request.Description);
    
    public static partial ContactResponse ToResponse(this ContactEntity entity);
}
