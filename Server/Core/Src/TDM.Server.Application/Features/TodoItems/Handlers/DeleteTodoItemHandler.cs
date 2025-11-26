using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Domain.Common;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.TodoItems.Commands;

namespace TDM.Server.Application.Features.TodoItems.Handlers;

public class DeleteTodoItemHandler : IRequestHandler<DeleteTodoItemCommand, bool>
{
    private readonly ILogger<DeleteTodoItemHandler> _logger;
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteTodoItemCommand> _validator;

    public DeleteTodoItemHandler(
        ILogger<DeleteTodoItemHandler> logger,
        ITodoItemRepository todoItemRepository,
        IUnitOfWork unitOfWork,
        IValidator<DeleteTodoItemCommand> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todoItemRepository = todoItemRepository ?? throw new ArgumentNullException(nameof(todoItemRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<bool> Handle(
        DeleteTodoItemCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Удаление задачи с ID: {Id}", request.Id);
        
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var todoItem = await _todoItemRepository.GetByIdAsync(request.Id, cancellationToken);

        if (todoItem == null)
        {
            _logger.LogWarning("Задача с ID {Id} не найдена.", request.Id);
            return false;
        }

        _todoItemRepository.Delete(todoItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Задача с ID {Id} успешно удалена.", request.Id);

        return true;
    }
}
