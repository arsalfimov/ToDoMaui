using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.TodoItems;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.TodoItems.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.TodoItems.Handlers;

public class GetTodayTodoItemsHandler : IRequestHandler<GetTodayTodoItemsQuery, IReadOnlyCollection<TodoItemResponse>>
{
    private readonly ILogger<GetTodayTodoItemsHandler> _logger;
    private readonly ITodoItemRepository _todoItemRepository;

    public GetTodayTodoItemsHandler(
        ILogger<GetTodayTodoItemsHandler> logger,
        ITodoItemRepository todoItemRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todoItemRepository = todoItemRepository ?? throw new ArgumentNullException(nameof(todoItemRepository));
    }

    public async Task<IReadOnlyCollection<TodoItemResponse>> Handle(
        GetTodayTodoItemsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение задач на сегодня...");

        var todoItems = await _todoItemRepository.GetTodayAsync(cancellationToken);

        if (todoItems.Count == 0)
        {
            _logger.LogWarning("Задач на сегодня не найдено.");
        }

        _logger.LogInformation("Найдено {Count} задач на сегодня.", todoItems.Count);

        return todoItems.Select(t => t.ToResponse()).ToList();
    }
}
