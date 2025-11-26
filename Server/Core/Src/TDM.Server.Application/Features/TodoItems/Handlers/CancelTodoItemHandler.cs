using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.TodoItems;
using TDM.Domain.Common;
using TDM.Domain.Repositories;
using TDM.Server.Application.Exceptions;
using TDM.Server.Application.Features.TodoItems.Commands;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.TodoItems.Handlers;

public class CancelTodoItemHandler : IRequestHandler<CancelTodoItemCommand, TodoItemResponse>
{
    private readonly ILogger<CancelTodoItemHandler> _logger;
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CancelTodoItemCommand> _validator;

    public CancelTodoItemHandler(
        ILogger<CancelTodoItemHandler> logger,
        ITodoItemRepository todoItemRepository,
        IUnitOfWork unitOfWork,
        IValidator<CancelTodoItemCommand> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todoItemRepository = todoItemRepository ?? throw new ArgumentNullException(nameof(todoItemRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<TodoItemResponse> Handle(
        CancelTodoItemCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Отметка задачи {Id} как отменённой.", request.Id);

        // Валидация
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var todoItem = await _todoItemRepository.GetByIdAsync(request.Id, cancellationToken);

        if (todoItem == null)
        {
            _logger.LogWarning("Задача с ID {Id} не найдена.", request.Id);
            throw new NotFoundException("Задача", request.Id);
        }

        todoItem.Cancel();

        _todoItemRepository.Update(todoItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Задача с ID {Id} отмечена как отменённая.", request.Id);

        return todoItem.ToResponse();
    }
}
