using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TDM.Api.Contracts.Users;
using TDM.Server.Application.Features.Users.Commands;
using TDM.Server.Application.Features.Users.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.API.Controllers;

/// <summary>
/// Контроллер для управления пользователями
/// </summary>
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Получить пользователя по ID
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<UserResponse>> GetUserById(long id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        UserResponse response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<UserResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyCollection<UserResponse>>> GetAllUsers(CancellationToken cancellationToken)
    {
        var query = new GetAllUsersQuery();
        IReadOnlyCollection<UserResponse> response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Получить пользователей по роли
    /// </summary>
    [HttpGet("by-role/{roleId:long}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<UserResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyCollection<UserResponse>>> GetUsersByRoleId(long roleId, CancellationToken cancellationToken)
    {
        var query = new GetUsersByRoleIdQuery(roleId);
        IReadOnlyCollection<UserResponse> response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Вход пользователя
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginUserResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<LoginUserResponse>> LoginUser([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        LoginUserCommand command = request.ToCommand();
        LoginUserResponse response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Создать нового пользователя
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        CreateUserCommand command = request.ToCommand();
        UserResponse response = await _mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(
            nameof(GetUserById),
            new { id = response.Id },
            response);
    }

    /// <summary>
    /// Обновить пользователя
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<ActionResult<UserResponse>> UpdateUser(long id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        UpdateUserCommand command = request.ToCommand(id);
        UserResponse response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Удалить пользователя
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeleteUser(long id, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удалить несколько пользователей
    /// </summary>
    [HttpDelete("delete-range")]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<int>> DeleteMultipleUsers([FromBody] IReadOnlyCollection<long> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeUserCommand(ids);
        int deletedCount = await _mediator.Send(command, cancellationToken);
        return Ok(deletedCount);
    }
}
