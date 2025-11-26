using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.Users;
using TDM.Domain.Entities;
using TDM.Domain.Repositories;
using TDM.Server.Application.Exceptions;
using TDM.Server.Application.Features.Users.Queries;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.Users.Handlers;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
{
    private readonly ILogger<GetUserByIdHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<GetUserByIdQuery> _validator;

    public GetUserByIdHandler(
        ILogger<GetUserByIdHandler> logger,
        IUserRepository userRepository,
        IValidator<GetUserByIdQuery> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Начата обработка запроса на получение пользователя с Id: {Id}", request.Id);

        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        UserEntity? user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("Пользователь с Id: {Id} не найден", request.Id);
            throw new NotFoundException("Пользователь", request.Id);
        }

        return user.ToResponse();
    }
}
