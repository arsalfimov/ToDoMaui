using MediatR;
using TDM.Api.Contracts.TodoItems;

namespace TDM.Server.Application.Features.TodoItems.Commands;

public record CancelTodoItemCommand(long Id) : IRequest<TodoItemResponse>;
