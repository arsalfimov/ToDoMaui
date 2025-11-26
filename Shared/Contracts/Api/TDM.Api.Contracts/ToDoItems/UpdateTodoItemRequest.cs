using TDM.Api.Enum;

namespace TDM.Api.Contracts.TodoItems;

public record UpdateTodoItemRequest(
    string Title,
    string? Details = null,
    long? DueDate = null,
    TodoStatus Status = TodoStatus.NotStarted,
    Priority Priority = Priority.Low,
    long? ContactId = null,
    string? Description = null);

