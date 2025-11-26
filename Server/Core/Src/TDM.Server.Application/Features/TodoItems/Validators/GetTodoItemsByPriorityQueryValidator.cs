using FluentValidation;
using TDM.Server.Application.Features.TodoItems.Queries;

namespace TDM.Server.Application.Features.TodoItems.Validators;

public class GetTodoItemsByPriorityQueryValidator : AbstractValidator<GetTodoItemsByPriorityQuery>
{
    public GetTodoItemsByPriorityQueryValidator()
    {
        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Некорректное значение приоритета.");
    }
}
