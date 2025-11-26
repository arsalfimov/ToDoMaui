using MediatR;
using TDM.Api.Contracts.TodoItems;
using TDM.Api.Enum;

namespace TDM.Server.Application.Features.TodoItems.Queries;

public record GetTodoItemsByPriorityQuery(Priority Priority) : IRequest<IReadOnlyCollection<TodoItemResponse>>;
