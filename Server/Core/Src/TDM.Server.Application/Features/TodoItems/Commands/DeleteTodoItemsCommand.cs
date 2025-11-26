using MediatR;

namespace TDM.Server.Application.Features.TodoItems.Commands;

public record DeleteTodoItemsCommand(IReadOnlyCollection<long> Ids) : IRequest<int>;
