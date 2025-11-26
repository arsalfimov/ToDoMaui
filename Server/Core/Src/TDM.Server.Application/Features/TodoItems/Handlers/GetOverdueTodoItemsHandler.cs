using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.TodoItems;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.TodoItems.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.TodoItems.Handlers;

public class GetOverdueTodoItemsHandler : IRequestHandler<GetOverdueTodoItemsQuery, IReadOnlyCollection<TodoItemResponse>>
{
    private readonly ILogger<GetOverdueTodoItemsHandler> _logger;
    private readonly ITodoItemRepository _todoItemRepository;

    public GetOverdueTodoItemsHandler(
        ILogger<GetOverdueTodoItemsHandler> logger,
        ITodoItemRepository todoItemRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todoItemRepository = todoItemRepository ?? throw new ArgumentNullException(nameof(todoItemRepository));
    }

    public async Task<IReadOnlyCollection<TodoItemResponse>> Handle(
        GetOverdueTodoItemsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение просроченных задач...");

        var todoItems = await _todoItemRepository.GetOverdueAsync(cancellationToken);

        if (todoItems.Count == 0)
        {
            _logger.LogWarning("Просроченные задачи не найдены.");
        }

        _logger.LogInformation("Найдено {Count} просроченных задач.", todoItems.Count);

        return todoItems.Select(t => t.ToResponse()).ToList();
    }
}
