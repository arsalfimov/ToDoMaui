using MediatR;
using TDM.Api.Contracts.TodoItems;

namespace TDM.Server.Application.Features.TodoItems.Commands;

public record CompleteTodoItemCommand(long Id) : IRequest<TodoItemResponse>;
