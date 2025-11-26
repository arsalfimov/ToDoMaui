using FluentValidation;
using TDM.Server.Application.Features.TodoItems.Queries;

namespace TDM.Server.Application.Features.TodoItems.Validators;

public class GetTodoItemsByStatusQueryValidator : AbstractValidator<GetTodoItemsByStatusQuery>
{
    public GetTodoItemsByStatusQueryValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Некорректное значение статуса.");
    }
}
