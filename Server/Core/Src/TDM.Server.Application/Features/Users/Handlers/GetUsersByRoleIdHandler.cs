using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.Users;
using TDM.Domain.Repositories;
using TDM.Server.Application.Features.Users.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.Users.Handlers;

public class GetUsersByRoleIdHandler : IRequestHandler<GetUsersByRoleIdQuery, IReadOnlyCollection<UserResponse>>
{
    private readonly ILogger<GetUsersByRoleIdHandler> _logger;
    private readonly IUserRepository _userRepository;

    public GetUsersByRoleIdHandler(
        ILogger<GetUsersByRoleIdHandler> logger,
        IUserRepository userRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<IReadOnlyCollection<UserResponse>> Handle(
        GetUsersByRoleIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Начата обработка запроса на получение пользователей по роли: {RoleId}", request.RoleId);

        var users = await _userRepository.GetByRoleIdAsync(request.RoleId, cancellationToken);

        _logger.LogInformation("Получено {Count} пользователей для роли {RoleId}", users.Count, request.RoleId);

        return users.Select(u => u.ToResponse()).ToList();
    }
}
