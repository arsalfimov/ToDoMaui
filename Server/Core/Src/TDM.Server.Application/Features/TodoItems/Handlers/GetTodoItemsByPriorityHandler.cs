using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.TodoItems;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.TodoItems.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.TodoItems.Handlers;

public class GetTodoItemsByPriorityHandler : IRequestHandler<GetTodoItemsByPriorityQuery, IReadOnlyCollection<TodoItemResponse>>
{
    private readonly ILogger<GetTodoItemsByPriorityHandler> _logger;
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IValidator<GetTodoItemsByPriorityQuery> _validator;

    public GetTodoItemsByPriorityHandler(
        ILogger<GetTodoItemsByPriorityHandler> logger,
        ITodoItemRepository todoItemRepository,
        IValidator<GetTodoItemsByPriorityQuery> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todoItemRepository = todoItemRepository ?? throw new ArgumentNullException(nameof(todoItemRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<IReadOnlyCollection<TodoItemResponse>> Handle(
        GetTodoItemsByPriorityQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение задач с приоритетом: {Priority}", request.Priority);
        
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var todoItems = await _todoItemRepository.GetByPriorityAsync(request.Priority, cancellationToken);

        if (todoItems.Count == 0)
        {
            _logger.LogWarning("Задачи с приоритетом {Priority} не найдены.", request.Priority);
        }

        _logger.LogInformation("Найдено {Count} задач с приоритетом {Priority}.", todoItems.Count, request.Priority);

        return todoItems.Select(t => t.ToResponse()).ToList();
    }
}
