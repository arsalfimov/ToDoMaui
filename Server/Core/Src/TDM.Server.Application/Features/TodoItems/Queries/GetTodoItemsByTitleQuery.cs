using MediatR;
using TDM.Api.Contracts.TodoItems;

namespace TDM.Server.Application.Features.TodoItems.Queries;

public record GetTodoItemsByTitleQuery(string Title) : IRequest<IReadOnlyCollection<TodoItemResponse>>;
