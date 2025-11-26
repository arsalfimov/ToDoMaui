using FluentValidation;
using TDM.Server.Application.Features.Contacts.Commands;

namespace TDM.Server.Application.Features.Contacts.Validators;

public class DeleteContactCommandValidator : AbstractValidator<DeleteContactCommand>
{
    public DeleteContactCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID контакта должно быть больше 0.");
    }
}
