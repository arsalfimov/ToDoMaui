using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Domain.Common;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.Contacts.Commands;

namespace TDM.Server.Application.Features.Contacts.Handlers;

public class DeleteContactHandler : IRequestHandler<DeleteContactCommand, bool>
{
    private readonly ILogger<DeleteContactHandler> _logger;
    private readonly IContactRepository _contactRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteContactCommand> _validator;

    public DeleteContactHandler(
        ILogger<DeleteContactHandler> logger,
        IContactRepository contactRepository,
        IUnitOfWork unitOfWork,
        IValidator<DeleteContactCommand> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<bool> Handle(
        DeleteContactCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Удаление контакта с ID: {Id}", request.Id);
        
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var contact = await _contactRepository.GetByIdAsync(request.Id, cancellationToken);

        if (contact == null)
        {
            _logger.LogWarning("Контакт с ID {Id} не найден.", request.Id);
            return false;
        }

        _contactRepository.Delete(contact);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Контакт с ID {Id} успешно удалён.", request.Id);

        return true;
    }
}
