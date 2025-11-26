using FluentValidation;
using TDM.Server.Application.Features.Contacts.Queries;

namespace TDM.Server.Application.Features.Contacts.Validators;

public class GetContactByIdQueryValidator : AbstractValidator<GetContactByIdQuery>
{
    public GetContactByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID контакта должно быть больше 0.");
    }
}
