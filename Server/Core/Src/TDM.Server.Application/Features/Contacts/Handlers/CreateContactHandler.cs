using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.Contacts;
using TDM.Domain.Common;
using TDM.Domain.Entities;
using TDM.Domain.Repositories;
using TDM.Server.Application.Exceptions;
using TDM.Server.Application.Features.Contacts.Commands;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.Contacts.Handlers;

public class CreateContactHandler : IRequestHandler<CreateContactCommand, ContactResponse>
{
    private readonly ILogger<CreateContactHandler> _logger;
    private readonly IContactRepository _contactRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateContactCommand> _validator;

    public CreateContactHandler(
        ILogger<CreateContactHandler> logger,
        IContactRepository contactRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateContactCommand> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<ContactResponse> Handle(
        CreateContactCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Создание нового контакта: {FirstName} {LastName}", request.FirstName, request.LastName);

        // Валидация
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        // Проверка на дубликат email
        await ValidateUniqueEmailAsync(request, cancellationToken);

        var contact = ContactEntity.Create(
            request.FirstName,
            request.LastName,
            request.Phone,
            request.Email,
            request.Address,
            request.Description);

        _contactRepository.Add(contact);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Контакт с ID {Id} успешно создан.", contact.Id);

        return contact.ToResponse();
    }

    private async Task ValidateUniqueEmailAsync(CreateContactCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return;

        var existingContact = await _contactRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (existingContact is not null)
        {
            _logger.LogWarning("Попытка создать контакт с уже существующим email: {Email}", request.Email);
            throw new ConflictException($"Контакт с email '{request.Email}' уже существует.");
        }
    }
}
