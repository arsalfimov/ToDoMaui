using MediatR;
using TDM.Api.Contracts.TodoItems;
using TDM.Api.Enum;

namespace TDM.Server.Application.Features.TodoItems.Commands;

public record UpdateTodoItemCommand(
    long Id,
    string Title,
    string? Details = null,
    long? DueDate = null,
    TodoStatus Status = TodoStatus.NotStarted,
    Priority Priority = Priority.Low,
    long? ContactId = null,
    string? Description = null) : IRequest<TodoItemResponse>;
