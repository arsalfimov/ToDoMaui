using FluentValidation;
using TDM.Server.Application.Common;
using TDM.Server.Application.Features.Contacts.Commands;

namespace TDM.Server.Application.Features.Contacts.Validators;

public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
{
    public CreateContactCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Имя контакта обязательно.")
            .MaximumLength(100).WithMessage("Имя контакта не должно превышать 100 символов.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Фамилия контакта обязательна.")
            .MaximumLength(100).WithMessage("Фамилия контакта не должна превышать 100 символов.");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("Телефон не должен превышать 20 символов.")
            .Matches(RegexPatterns.Phone).WithMessage("Некорректный формат номера телефона. Пример: +7 (999) 999-99-99")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.Email)
            .MaximumLength(200).WithMessage("Email не должен превышать 200 символов.")
            .EmailAddress().WithMessage("Некорректный формат email.")
            .Matches(RegexPatterns.Email).WithMessage("Некорректный формат email адреса.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Адрес не должен превышать 500 символов.")
            .When(x => !string.IsNullOrWhiteSpace(x.Address));
    }
}
