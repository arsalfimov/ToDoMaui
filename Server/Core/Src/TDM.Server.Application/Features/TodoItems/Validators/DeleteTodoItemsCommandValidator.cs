using FluentValidation;
using TDM.Server.Application.Features.TodoItems.Commands;

namespace TDM.Server.Application.Features.TodoItems.Validators;
public class DeleteTodoItemsCommandValidator : AbstractValidator<DeleteTodoItemsCommand>
{
    public DeleteTodoItemsCommandValidator()
    {
        RuleFor(x => x.Ids)
            .NotEmpty().WithMessage("Список ID не может быть пустым.")
            .Must(ids => ids.All(id => id > 0)).WithMessage("Все ID должны быть больше 0.");
    }
}
