using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TDM.Api.Contracts.Contacts;
using TDM.Server.Application.Features.Contacts.Commands;
using TDM.Server.Application.Features.Contacts.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.API.Controllers;

[ApiController]
[Route("api/contacts")]
public class ContactsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContactsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ContactResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyCollection<ContactResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllContactsQuery();
        IReadOnlyCollection<ContactResponse> response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ContactResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ContactResponse>> GetById(long id, CancellationToken cancellationToken)
    {
        var query = new GetContactByIdQuery(id);
        ContactResponse response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet("search/name")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ContactResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyCollection<ContactResponse>>> SearchByName([FromQuery] string name, CancellationToken cancellationToken)
    {
        var query = new GetContactsByNameQuery(name);
        IReadOnlyCollection<ContactResponse> response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet("search/email")]
    [ProducesResponseType(typeof(ContactResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ContactResponse>> GetByEmail([FromQuery] string email, CancellationToken cancellationToken)
    {
        var query = new GetContactByEmailQuery(email);
        ContactResponse response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ContactResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<ActionResult<ContactResponse>> Create([FromBody] CreateContactRequest request, CancellationToken cancellationToken)
    {
        CreateContactCommand command = request.ToCommand();
        ContactResponse response = await _mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response);
    }
    
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ContactResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<ActionResult<ContactResponse>> Update(long id, [FromBody] UpdateContactRequest request, CancellationToken cancellationToken)
    {
        UpdateContactCommand command = request.ToCommand(id);
        ContactResponse response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var command = new DeleteContactCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("delete-range")]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<int>> DeleteMultiple([FromBody] IReadOnlyCollection<long> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteContactsCommand(ids);
        int deletedCount = await _mediator.Send(command, cancellationToken);
        return Ok(deletedCount);
    }
}
