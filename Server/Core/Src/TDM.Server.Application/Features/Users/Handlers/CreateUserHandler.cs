using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TDM.Api.Contracts.Users;
using TDM.Domain.Common;
using TDM.Domain.Entities;
using TDM.Domain.Repositories;
using TDM.Server.Application.Common;
using TDM.Server.Application.Exceptions;
using TDM.Server.Application.Features.Users.Commands;
using TDM.Server.Application.Mappings;

namespace TDM.Server.Application.Features.Users.Handlers;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserResponse>
{
    private readonly ILogger<CreateUserHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasherService _passwordHasher;
    private readonly IValidator<CreateUserCommand> _validator;

    public CreateUserHandler(
        ILogger<CreateUserHandler> logger,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasherService passwordHasher,
        IValidator<CreateUserCommand> validator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Начата обработка команды на создание пользователя с логином: {Login}", request.Login);

        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        await ValidateUniqueFieldsAsync(request, cancellationToken);

        RoleEntity role = await GetValidatedRoleAsync(request.RoleId, cancellationToken);
        UserEntity createdUser = await CreateUserAsync(request, role, cancellationToken);

        _logger.LogInformation("Пользователь '{Login}' успешно создан с Id: {Id}", createdUser.Login, createdUser.Id);

        return createdUser.ToResponse();
    }

    private async Task ValidateUniqueFieldsAsync(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        UserEntity? existingUser = await _userRepository.GetByLoginAsync(request.Login, cancellationToken);

        if (existingUser is not null)
        {
            _logger.LogWarning("Попытка создать пользователя с уже существующим логином: {Login}", request.Login);
            throw new ConflictException($"Пользователь с логином: '{request.Login}' уже существует.");
        }
    }

    private async Task<RoleEntity> GetValidatedRoleAsync(long roleId, CancellationToken cancellationToken = default)
    {
        RoleEntity? role = await _roleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role is not null)
        {
            return role;
        }

        _logger.LogWarning("Попытка создать пользователя с несуществующей Id роли: {RoleId}", roleId);
        throw new BadRequestException($"Роль с Id: '{roleId}' не найдена.");
    }

    private async Task<UserEntity> CreateUserAsync(
        CreateUserCommand request,
        RoleEntity role,
        CancellationToken cancellationToken = default)
    {
        string passwordHash = _passwordHasher.Hash(request.Password);

        UserEntity entity = UserEntity.Create(
            request.Login,
            passwordHash,
            false,
            request.RoleId,
            request.Description);

        entity.Role = role;

        _userRepository.Add(entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
