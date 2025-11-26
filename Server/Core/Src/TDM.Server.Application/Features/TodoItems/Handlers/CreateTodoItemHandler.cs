using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.TodoItems;
using TDM.Domain.Common;
using TDM.Domain.Entities;
using TDM.Domain.Repositories;
using TDM.Server.Application.Exceptions;
using TDM.Server.Application.Features.TodoItems.Commands;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.TodoItems.Handlers;

public class CreateTodoItemHandler : IRequestHandler<CreateTodoItemCommand, TodoItemResponse>
{
    private readonly ILogger<CreateTodoItemHandler> _logger;
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IContactRepository _contactRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateTodoItemCommand> _validator;

    public CreateTodoItemHandler(
        ILogger<CreateTodoItemHandler> logger,
        ITodoItemRepository todoItemRepository,
        IContactRepository contactRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateTodoItemCommand> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todoItemRepository = todoItemRepository ?? throw new ArgumentNullException(nameof(todoItemRepository));
        _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<TodoItemResponse> Handle(
        CreateTodoItemCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Создание новой задачи с заголовком: {Title}", request.Title);
        
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        await ValidateContactExistsAsync(request.ContactId, cancellationToken);

        var todoItem = TodoItemEntity.Create(
            request.Title,
            request.Details,
            request.DueDate,
            request.Status,
            request.Priority,
            request.ContactId,
            request.Description);

        _todoItemRepository.Add(todoItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Задача с ID {Id} успешно создана.", todoItem.Id);

        return todoItem.ToResponse();
    }

    private async Task ValidateContactExistsAsync(long? contactId, CancellationToken cancellationToken)
    {
        if (!contactId.HasValue)
            return;

        var contact = await _contactRepository.GetByIdAsync(contactId.Value, cancellationToken);

        if (contact is null)
        {
            _logger.LogWarning("Попытка создать задачу с несуществующим контактом ID: {ContactId}", contactId.Value);
            throw new BadRequestException($"Контакт с ID {contactId.Value} не найден.");
        }
    }
}
