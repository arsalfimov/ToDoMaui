using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.TodoItems;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.TodoItems.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.TodoItems.Handlers;

public class GetTodoItemsByContactIdHandler : IRequestHandler<GetTodoItemsByContactIdQuery, IReadOnlyCollection<TodoItemResponse>>
{
    private readonly ILogger<GetTodoItemsByContactIdHandler> _logger;
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IValidator<GetTodoItemsByContactIdQuery> _validator;

    public GetTodoItemsByContactIdHandler(
        ILogger<GetTodoItemsByContactIdHandler> logger,
        ITodoItemRepository todoItemRepository,
        IValidator<GetTodoItemsByContactIdQuery> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todoItemRepository = todoItemRepository ?? throw new ArgumentNullException(nameof(todoItemRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<IReadOnlyCollection<TodoItemResponse>> Handle(
        GetTodoItemsByContactIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение задач для контакта с ID: {ContactId}", request.ContactId);
        
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var todoItems = await _todoItemRepository.GetByContactIdAsync(request.ContactId, cancellationToken);

        if (todoItems.Count == 0)
        {
            _logger.LogWarning("Задачи для контакта с ID {ContactId} не найдены.", request.ContactId);
        }

        _logger.LogInformation("Найдено {Count} задач для контакта с ID {ContactId}.", todoItems.Count, request.ContactId);

        return todoItems.Select(t => t.ToResponse()).ToList();
    }
}
