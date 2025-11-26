namespace TDM.Api.Contracts.TodoItems;

public record TodoItemResponse(
    long Id,
    string Title,
    string? Details,
    long? DueDate,
    int Status,
    int Priority,
    long? ContactId,
    string? ContactName,
    long? CompletedAt,
    string? Description,
    long CreatedAt,
    long UpdatedAt);
