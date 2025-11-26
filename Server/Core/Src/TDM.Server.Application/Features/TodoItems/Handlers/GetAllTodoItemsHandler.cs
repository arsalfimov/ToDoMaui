using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.TodoItems;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.TodoItems.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.TodoItems.Handlers;

public class GetAllTodoItemsHandler : IRequestHandler<GetAllTodoItemsQuery, IReadOnlyCollection<TodoItemResponse>>
{
    private readonly ILogger<GetAllTodoItemsHandler> _logger;
    private readonly ITodoItemRepository _todoItemRepository;

    public GetAllTodoItemsHandler(
        ILogger<GetAllTodoItemsHandler> logger,
        ITodoItemRepository todoItemRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todoItemRepository = todoItemRepository ?? throw new ArgumentNullException(nameof(todoItemRepository));
    }

    public async Task<IReadOnlyCollection<TodoItemResponse>> Handle(
        GetAllTodoItemsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение всех задач...");

        var todoItems = await _todoItemRepository.GetAllAsync(cancellationToken);

        if (todoItems.Count == 0)
        {
            _logger.LogWarning("Задачи не найдены. Возвращен пустой список.");
        }

        _logger.LogInformation("Найдено {Count} задач.", todoItems.Count);

        return todoItems.Select(t => t.ToResponse()).ToList();
    }
}
