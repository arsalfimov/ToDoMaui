using MediatR;

namespace TDM.Server.Application.Features.TodoItems.Commands;

public record DeleteTodoItemCommand(long Id) : IRequest<bool>;
