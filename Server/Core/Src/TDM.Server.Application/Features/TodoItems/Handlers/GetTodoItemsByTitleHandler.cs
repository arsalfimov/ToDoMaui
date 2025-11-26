using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.TodoItems;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.TodoItems.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.TodoItems.Handlers;

public class GetTodoItemsByTitleHandler : IRequestHandler<GetTodoItemsByTitleQuery, IReadOnlyCollection<TodoItemResponse>>
{
    private readonly ILogger<GetTodoItemsByTitleHandler> _logger;
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IValidator<GetTodoItemsByTitleQuery> _validator;

    public GetTodoItemsByTitleHandler(
        ILogger<GetTodoItemsByTitleHandler> logger,
        ITodoItemRepository todoItemRepository,
        IValidator<GetTodoItemsByTitleQuery> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todoItemRepository = todoItemRepository ?? throw new ArgumentNullException(nameof(todoItemRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<IReadOnlyCollection<TodoItemResponse>> Handle(
        GetTodoItemsByTitleQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Поиск задач по заголовку: {Title}", request.Title);
        
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var todoItems = await _todoItemRepository.GetByTitleAsync(request.Title, cancellationToken);

        if (todoItems.Count == 0)
        {
            _logger.LogWarning("Задачи с заголовком {Title} не найдены.", request.Title);
        }

        _logger.LogInformation("Найдено {Count} задач с заголовком {Title}.", todoItems.Count, request.Title);

        return todoItems.Select(t => t.ToResponse()).ToList();
    }
}
