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

public class UpdateTodoItemHandler : IRequestHandler<UpdateTodoItemCommand, TodoItemResponse>
{
    private readonly ILogger<UpdateTodoItemHandler> _logger;
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IContactRepository _contactRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateTodoItemCommand> _validator;

    public UpdateTodoItemHandler(
        ILogger<UpdateTodoItemHandler> logger,
        ITodoItemRepository todoItemRepository,
        IContactRepository contactRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateTodoItemCommand> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todoItemRepository = todoItemRepository ?? throw new ArgumentNullException(nameof(todoItemRepository));
        _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<TodoItemResponse> Handle(
        UpdateTodoItemCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Обновление задачи с ID: {Id}", request.Id);
        
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var todoItem = await _todoItemRepository.GetByIdAsync(request.Id, cancellationToken);

        if (todoItem == null)
        {
            _logger.LogWarning("Задача с ID {Id} не найдена.", request.Id);
            throw new NotFoundException("Задача", request.Id);
        }
        
        await ValidateContactExistsAsync(request.ContactId, cancellationToken);

        todoItem.Update(
            request.Title,
            request.Details,
            request.DueDate,
            request.Status,
            request.Priority,
            request.ContactId,
            request.Description);

        _todoItemRepository.Update(todoItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Задача с ID {Id} успешно обновлена.", todoItem.Id);

        return todoItem.ToResponse();
    }

    private async Task ValidateContactExistsAsync(long? contactId, CancellationToken cancellationToken)
    {
        if (!contactId.HasValue)
            return;

        var contact = await _contactRepository.GetByIdAsync(contactId.Value, cancellationToken);

        if (contact is null)
        {
            _logger.LogWarning("Попытка обновить задачу с несуществующим контактом ID: {ContactId}", contactId.Value);
            throw new BadRequestException($"Контакт с ID {contactId.Value} не найден.");
        }
    }
}
