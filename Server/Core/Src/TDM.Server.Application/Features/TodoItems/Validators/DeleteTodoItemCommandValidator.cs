using FluentValidation;
using TDM.Server.Application.Features.TodoItems.Commands;

namespace TDM.Server.Application.Features.TodoItems.Validators;

public class DeleteTodoItemCommandValidator : AbstractValidator<DeleteTodoItemCommand>
{
    public DeleteTodoItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID задачи должно быть больше 0.");
    }
}
