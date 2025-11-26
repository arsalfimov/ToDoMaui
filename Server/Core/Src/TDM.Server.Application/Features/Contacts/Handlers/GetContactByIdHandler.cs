using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.Contacts;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.Contacts.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.Contacts.Handlers;

public class GetContactByIdHandler : IRequestHandler<GetContactByIdQuery, ContactResponse?>
{
    private readonly ILogger<GetContactByIdHandler> _logger;
    private readonly IContactRepository _contactRepository;
    private readonly IValidator<GetContactByIdQuery> _validator;

    public GetContactByIdHandler(
        ILogger<GetContactByIdHandler> logger,
        IContactRepository contactRepository,
        IValidator<GetContactByIdQuery> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<ContactResponse?> Handle(
        GetContactByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение контакта с ID: {Id}", request.Id);
        
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var contact = await _contactRepository.GetByIdAsync(request.Id, cancellationToken);

        if (contact == null)
        {
            _logger.LogWarning("Контакт с ID {Id} не найден.", request.Id);
            return null;
        }

        _logger.LogInformation("Контакт с ID {Id} найден.", request.Id);

        return contact.ToResponse();
    }
}
