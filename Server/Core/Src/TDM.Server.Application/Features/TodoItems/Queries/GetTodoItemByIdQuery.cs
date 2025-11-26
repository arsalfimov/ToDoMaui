using MediatR;
using TDM.Api.Contracts.TodoItems;

namespace TDM.Server.Application.Features.TodoItems.Queries;

public record GetTodoItemByIdQuery(long Id) : IRequest<TodoItemResponse?>;
