using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.TodoItems;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.TodoItems.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.TodoItems.Handlers;

public class GetTodoItemByIdHandler : IRequestHandler<GetTodoItemByIdQuery, TodoItemResponse?>
{
    private readonly ILogger<GetTodoItemByIdHandler> _logger;
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IValidator<GetTodoItemByIdQuery> _validator;

    public GetTodoItemByIdHandler(
        ILogger<GetTodoItemByIdHandler> logger,
        ITodoItemRepository todoItemRepository,
        IValidator<GetTodoItemByIdQuery> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todoItemRepository = todoItemRepository ?? throw new ArgumentNullException(nameof(todoItemRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<TodoItemResponse?> Handle(
        GetTodoItemByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение задачи с ID: {Id}", request.Id);
        
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var todoItem = await _todoItemRepository.GetByIdAsync(request.Id, cancellationToken);

        if (todoItem == null)
        {
            _logger.LogWarning("Задача с ID {Id} не найдена.", request.Id);
            return null;
        }

        _logger.LogInformation("Задача с ID {Id} найдена.", request.Id);

        return todoItem.ToResponse();
    }
}
