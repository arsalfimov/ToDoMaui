using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Domain.Common;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.Contacts.Commands;

namespace TDM.Server.Application.Features.Contacts.Handlers;

public class DeleteContactsHandler : IRequestHandler<DeleteContactsCommand, int>
{
    private readonly ILogger<DeleteContactsHandler> _logger;
    private readonly IContactRepository _contactRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteContactsCommand> _validator;

    public DeleteContactsHandler(
        ILogger<DeleteContactsHandler> logger,
        IContactRepository contactRepository,
        IUnitOfWork unitOfWork,
        IValidator<DeleteContactsCommand> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<int> Handle(
        DeleteContactsCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Удаление {Count} контактов.", request.Ids.Count);
        
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        await _contactRepository.DeleteRangeAsync(request.Ids, cancellationToken);
        var deletedCount = await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Успешно удалено {Count} контактов.", deletedCount);

        return deletedCount;
    }
}
