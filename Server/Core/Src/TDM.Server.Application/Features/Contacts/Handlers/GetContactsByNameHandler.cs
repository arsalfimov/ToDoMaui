using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.Contacts;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.Contacts.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.Contacts.Handlers;

public class GetContactsByNameHandler : IRequestHandler<GetContactsByNameQuery, IReadOnlyCollection<ContactResponse>>
{
    private readonly ILogger<GetContactsByNameHandler> _logger;
    private readonly IContactRepository _contactRepository;
    private readonly IValidator<GetContactsByNameQuery> _validator;

    public GetContactsByNameHandler(
        ILogger<GetContactsByNameHandler> logger,
        IContactRepository contactRepository,
        IValidator<GetContactsByNameQuery> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<IReadOnlyCollection<ContactResponse>> Handle(
        GetContactsByNameQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Поиск контактов по имени: {Name}", request.Name);
        
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var contacts = await _contactRepository.GetByNameAsync(request.Name, cancellationToken);

        if (contacts.Count == 0)
        {
            _logger.LogWarning("Контакты с именем {Name} не найдены.", request.Name);
        }

        _logger.LogInformation("Найдено {Count} контактов с именем {Name}.", contacts.Count, request.Name);

        return contacts.Select(c => c.ToResponse()).ToList();
    }
}
