using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.Users;
using TDM.Domain.Entities;
using TDM.Domain.Repositories;
using TDM.Server.Application.Common;
using TDM.Server.Application.Exceptions;
using TDM.Server.Application.Features.Users.Commands;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.Users.Handlers;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly ILogger<LoginUserHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasherService _passwordHasher;
    private readonly IValidator<LoginUserCommand> _validator;

    public LoginUserHandler(
        ILogger<LoginUserHandler> logger,
        IUserRepository userRepository,
        IPasswordHasherService passwordHasher,
        IValidator<LoginUserCommand> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Начата обработка запроса на авторизацию пользователя с логином: {Login}", request.Login);

        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        UserEntity? user = await _userRepository.GetByLoginAsync(request.Login, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("Пользователь с логином: {Login} не найден", request.Login);
            throw new NotFoundException($"Пользователь с логином '{request.Login}' не найден.");
        }

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Неверный пароль для пользователя с логином: {Login}", request.Login);
            throw new BadRequestException("Неверный пароль для этого пользователя.");
        }

        if (user.IsBlocked)
        {
            _logger.LogWarning("Попытка входа заблокированного пользователя: {Login}", request.Login);
            throw new BadRequestException("Пользователь заблокирован.");
        }

        _logger.LogInformation("Пользователь с логином: {Login} успешно авторизован", request.Login);

        return user.ToLoginResponse();
    }
}
