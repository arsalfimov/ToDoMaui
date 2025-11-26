using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.TodoItems;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.TodoItems.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.TodoItems.Handlers;

public class GetTodoItemsByStatusHandler : IRequestHandler<GetTodoItemsByStatusQuery, IReadOnlyCollection<TodoItemResponse>>
{
    private readonly ILogger<GetTodoItemsByStatusHandler> _logger;
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IValidator<GetTodoItemsByStatusQuery> _validator;

    public GetTodoItemsByStatusHandler(
        ILogger<GetTodoItemsByStatusHandler> logger,
        ITodoItemRepository todoItemRepository,
        IValidator<GetTodoItemsByStatusQuery> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todoItemRepository = todoItemRepository ?? throw new ArgumentNullException(nameof(todoItemRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<IReadOnlyCollection<TodoItemResponse>> Handle(
        GetTodoItemsByStatusQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение задач со статусом: {Status}", request.Status);
        
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var todoItems = await _todoItemRepository.GetByStatusAsync(request.Status, cancellationToken);

        if (todoItems.Count == 0)
        {
            _logger.LogWarning("Задачи со статусом {Status} не найдены.", request.Status);
        }

        _logger.LogInformation("Найдено {Count} задач со статусом {Status}.", todoItems.Count, request.Status);

        return todoItems.Select(t => t.ToResponse()).ToList();
    }
}
