using FluentValidation;
using TDM.Server.Application.Features.Users.Commands;

namespace TDM.Server.Application.Features.Users.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Логин обязателен.")
            .Length(3, 50).WithMessage("Длина логина должна быть от 3 до 50 символов.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен.")
            .MinimumLength(6).WithMessage("Пароль должен содержать минимум 6 символов.");

        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("Идентификатор роли должен быть больше 0.");
    }
}
