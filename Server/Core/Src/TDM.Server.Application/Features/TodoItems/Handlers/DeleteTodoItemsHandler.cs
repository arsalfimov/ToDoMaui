using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Domain.Common;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.TodoItems.Commands;

namespace TDM.Server.Application.Features.TodoItems.Handlers;

public class DeleteTodoItemsHandler : IRequestHandler<DeleteTodoItemsCommand, int>
{
    private readonly ILogger<DeleteTodoItemsHandler> _logger;
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteTodoItemsCommand> _validator;

    public DeleteTodoItemsHandler(
        ILogger<DeleteTodoItemsHandler> logger,
        ITodoItemRepository todoItemRepository,
        IUnitOfWork unitOfWork,
        IValidator<DeleteTodoItemsCommand> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _todoItemRepository = todoItemRepository ?? throw new ArgumentNullException(nameof(todoItemRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<int> Handle(
        DeleteTodoItemsCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Удаление {Count} задач.", request.Ids.Count);

        // Валидация
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        await _todoItemRepository.DeleteRangeAsync(request.Ids, cancellationToken);
        var deletedCount = await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Успешно удалено {Count} задач.", deletedCount);

        return deletedCount;
    }
}
