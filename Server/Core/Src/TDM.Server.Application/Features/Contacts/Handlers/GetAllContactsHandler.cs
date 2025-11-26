using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.Contacts;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.Contacts.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.Contacts.Handlers;

public class GetAllContactsHandler : IRequestHandler<GetAllContactsQuery, IReadOnlyCollection<ContactResponse>>
{
    private readonly ILogger<GetAllContactsHandler> _logger;
    private readonly IContactRepository _contactRepository;

    public GetAllContactsHandler(
        ILogger<GetAllContactsHandler> logger,
        IContactRepository contactRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
    }

    public async Task<IReadOnlyCollection<ContactResponse>> Handle(
        GetAllContactsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение всех контактов...");

        var contacts = await _contactRepository.GetAllAsync(cancellationToken);

        if (contacts.Count == 0)
        {
            _logger.LogWarning("Контакты не найдены. Возвращен пустой список.");
        }

        _logger.LogInformation("Найдено {Count} контактов.", contacts.Count);

        return contacts.Select(c => c.ToResponse()).ToList();
    }
}
