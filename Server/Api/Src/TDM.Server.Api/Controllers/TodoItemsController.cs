using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TDM.Api.Contracts.TodoItems;
using TDM.Api.Enum;
using TDM.Server.Application.Features.TodoItems.Commands;
using TDM.Server.Application.Features.TodoItems.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.API.Controllers;

[ApiController]
[Route("api/to-do-items")]
public class TodoItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodoItemsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<TodoItemResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyCollection<TodoItemResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllTodoItemsQuery();
        IReadOnlyCollection<TodoItemResponse> response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(TodoItemResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<TodoItemResponse>> GetById(long id, CancellationToken cancellationToken)
    {
        var query = new GetTodoItemByIdQuery(id);
        TodoItemResponse response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet("contact/{contactId:long}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TodoItemResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyCollection<TodoItemResponse>>> GetByContactId(long contactId, CancellationToken cancellationToken)
    {
        var query = new GetTodoItemsByContactIdQuery(contactId);
        IReadOnlyCollection<TodoItemResponse> response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TodoItemResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyCollection<TodoItemResponse>>> GetByStatus(TodoStatus status, CancellationToken cancellationToken)
    {
        var query = new GetTodoItemsByStatusQuery(status);
        IReadOnlyCollection<TodoItemResponse> response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet("priority/{priority}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TodoItemResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyCollection<TodoItemResponse>>> GetByPriority(Priority priority, CancellationToken cancellationToken)
    {
        var query = new GetTodoItemsByPriorityQuery(priority);
        IReadOnlyCollection<TodoItemResponse> response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("overdue")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TodoItemResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyCollection<TodoItemResponse>>> GetOverdue(CancellationToken cancellationToken)
    {
        var query = new GetOverdueTodoItemsQuery();
        IReadOnlyCollection<TodoItemResponse> response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet("today")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TodoItemResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyCollection<TodoItemResponse>>> GetToday(CancellationToken cancellationToken)
    {
        var query = new GetTodayTodoItemsQuery();
        IReadOnlyCollection<TodoItemResponse> response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TodoItemResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyCollection<TodoItemResponse>>> Search([FromQuery] string title, CancellationToken cancellationToken)
    {
        var query = new GetTodoItemsByTitleQuery(title);
        IReadOnlyCollection<TodoItemResponse> response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(TodoItemResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<TodoItemResponse>> Create([FromBody] CreateTodoItemRequest request, CancellationToken cancellationToken)
    {
        CreateTodoItemCommand command = request.ToCommand();
        TodoItemResponse response = await _mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response);
    }

    /// <summary>
    /// Обновить задачу
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(TodoItemResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<TodoItemResponse>> Update(long id, [FromBody] UpdateTodoItemRequest request, CancellationToken cancellationToken)
    {
        UpdateTodoItemCommand command = request.ToCommand(id);
        TodoItemResponse response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
    
    [HttpPut("{id:long}/complete")]
    [ProducesResponseType(typeof(TodoItemResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<TodoItemResponse>> Complete(long id, CancellationToken cancellationToken)
    {
        var command = new CompleteTodoItemCommand(id);
        TodoItemResponse response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
    
    [HttpPut("{id:long}/cancel")]
    [ProducesResponseType(typeof(TodoItemResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<TodoItemResponse>> Cancel(long id, CancellationToken cancellationToken)
    {
        var command = new CancelTodoItemCommand(id);
        TodoItemResponse response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
    
    [HttpDelete("{id:long}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var command = new DeleteTodoItemCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("delete-range")]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<int>> DeleteMultiple([FromBody] IReadOnlyCollection<long> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteTodoItemsCommand(ids);
        int deletedCount = await _mediator.Send(command, cancellationToken);
        return Ok(deletedCount);
    }
}
