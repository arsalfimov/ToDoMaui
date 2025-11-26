using FluentValidation;
using TDM.Server.Application.Features.Contacts.Queries;

namespace TDM.Server.Application.Features.Contacts.Validators;

public class GetContactsByNameQueryValidator : AbstractValidator<GetContactsByNameQuery>
{
    public GetContactsByNameQueryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя контакта не может быть пустым.")
            .MinimumLength(1).WithMessage("Имя должно содержать минимум 1 символ.")
            .MaximumLength(200).WithMessage("Имя не должно превышать 200 символов.");
    }
}
