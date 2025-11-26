using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.Contacts;
using TDM.Domain.Common;
using TDM.Domain.Repositories;
using TDM.Server.Application.Exceptions;
using TDM.Server.Application.Features.Contacts.Commands;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.Contacts.Handlers;

public class UpdateContactHandler : IRequestHandler<UpdateContactCommand, ContactResponse>
{
    private readonly ILogger<UpdateContactHandler> _logger;
    private readonly IContactRepository _contactRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateContactCommand> _validator;

    public UpdateContactHandler(
        ILogger<UpdateContactHandler> logger,
        IContactRepository contactRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateContactCommand> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<ContactResponse> Handle(
        UpdateContactCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Обновление контакта с ID: {Id}", request.Id);
        
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var contact = await _contactRepository.GetByIdAsync(request.Id, cancellationToken);

        if (contact == null)
        {
            _logger.LogWarning("Контакт с ID {Id} не найден.", request.Id);
            throw new NotFoundException("Контакт", request.Id);
        }
        
        await ValidateUniqueEmailAsync(request, contact, cancellationToken);

        contact.Update(
            request.FirstName,
            request.LastName,
            request.Phone,
            request.Email,
            request.Address,
            request.Description);

        _contactRepository.Update(contact);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Контакт с ID {Id} успешно обновлен.", contact.Id);

        return contact.ToResponse();
    }

    private async Task ValidateUniqueEmailAsync(
        UpdateContactCommand request, 
        Domain.Entities.ContactEntity currentContact,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return;
        
        if (request.Email == currentContact.Email)
            return;

        var existingContact = await _contactRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (existingContact is not null)
        {
            _logger.LogWarning("Попытка обновить контакт с уже существующим email: {Email}", request.Email);
            throw new ConflictException($"Контакт с email '{request.Email}' уже существует.");
        }
    }
}
