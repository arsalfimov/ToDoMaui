using MediatR;
using TDM.Api.Contracts.TodoItems;

namespace TDM.Server.Application.Features.TodoItems.Queries;

public record GetTodoItemsByContactIdQuery(long ContactId) : IRequest<IReadOnlyCollection<TodoItemResponse>>;
