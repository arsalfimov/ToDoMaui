using FluentValidation;
using TDM.Server.Application.Features.TodoItems.Commands;

namespace TDM.Server.Application.Features.TodoItems.Validators;

public class CancelTodoItemCommandValidator : AbstractValidator<CancelTodoItemCommand>
{
    public CancelTodoItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID задачи должно быть больше 0.");
    }
}
