using FluentValidation;
using TDM.Server.Application.Features.Contacts.Commands;

namespace TDM.Server.Application.Features.Contacts.Validators;

public class DeleteContactsCommandValidator : AbstractValidator<DeleteContactsCommand>
{
    public DeleteContactsCommandValidator()
    {
        RuleFor(x => x.Ids)
            .NotEmpty().WithMessage("Список ID не может быть пустым.")
            .Must(ids => ids.All(id => id > 0)).WithMessage("Все ID должны быть больше 0.");
    }
}
