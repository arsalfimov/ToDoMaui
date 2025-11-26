using FluentValidation;
using TDM.Server.Application.Features.TodoItems.Queries;

namespace TDM.Server.Application.Features.TodoItems.Validators;

public class GetTodoItemsByTitleQueryValidator : AbstractValidator<GetTodoItemsByTitleQuery>
{
    public GetTodoItemsByTitleQueryValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Заголовок не может быть пустым.")
            .MinimumLength(1).WithMessage("Заголовок должен содержать минимум 1 символ.")
            .MaximumLength(200).WithMessage("Заголовок не должен превышать 200 символов.");
    }
}
