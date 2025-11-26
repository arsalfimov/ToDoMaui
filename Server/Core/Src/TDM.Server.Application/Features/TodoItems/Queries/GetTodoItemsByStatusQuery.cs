using MediatR;
using TDM.Api.Contracts.TodoItems;
using TDM.Api.Enum;

namespace TDM.Server.Application.Features.TodoItems.Queries;

public record GetTodoItemsByStatusQuery(TodoStatus Status) : IRequest<IReadOnlyCollection<TodoItemResponse>>;
