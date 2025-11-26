using FluentValidation;
using TDM.Server.Application.Features.Contacts.Queries;

namespace TDM.Server.Application.Features.Contacts.Validators;

public class GetContactByEmailQueryValidator : AbstractValidator<GetContactByEmailQuery>
{
    public GetContactByEmailQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обязателен.")
            .EmailAddress().WithMessage("Некорректный формат email.")
            .MaximumLength(200).WithMessage("Email не должен превышать 200 символов.");
    }
}
