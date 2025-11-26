using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.Contacts;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.Contacts.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.Contacts.Handlers;

public class GetContactByEmailHandler : IRequestHandler<GetContactByEmailQuery, ContactResponse?>
{
    private readonly ILogger<GetContactByEmailHandler> _logger;
    private readonly IContactRepository _contactRepository;
    private readonly IValidator<GetContactByEmailQuery> _validator;

    public GetContactByEmailHandler(
        ILogger<GetContactByEmailHandler> logger,
        IContactRepository contactRepository,
        IValidator<GetContactByEmailQuery> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<ContactResponse?> Handle(
        GetContactByEmailQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение контакта по email: {Email}", request.Email);
        
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var contact = await _contactRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (contact == null)
        {
            _logger.LogWarning("Контакт с email {Email} не найден.", request.Email);
            return null;
        }

        _logger.LogInformation("Контакт с email {Email} найден.", request.Email);

        return contact.ToResponse();
    }
}
