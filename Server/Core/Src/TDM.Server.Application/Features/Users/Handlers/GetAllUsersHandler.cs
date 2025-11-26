using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.Users;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.Users.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.Users.Handlers;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IReadOnlyCollection<UserResponse>>
{
    private readonly ILogger<GetAllUsersHandler> _logger;
    private readonly IUserRepository _userRepository;

    public GetAllUsersHandler(
        ILogger<GetAllUsersHandler> logger,
        IUserRepository userRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<IReadOnlyCollection<UserResponse>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Начата обработка запроса на получение всех пользователей");

        var users = await _userRepository.GetAllAsync(cancellationToken);

        _logger.LogInformation("Получено {Count} пользователей", users.Count);

        return users.Select(u => u.ToResponse()).ToList();
    }
}
