using FluentValidation;
using TDM.Server.Application.Features.TodoItems.Commands;

namespace TDM.Server.Application.Features.TodoItems.Validators;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Заголовок задачи обязателен.")
            .MaximumLength(200).WithMessage("Заголовок задачи не должен превышать 200 символов.");

        RuleFor(x => x.Details)
            .MaximumLength(2000).WithMessage("Описание задачи не должно превышать 2000 символов.")
            .When(x => !string.IsNullOrWhiteSpace(x.Details));

        RuleFor(x => x.ContactId)
            .GreaterThan(0).WithMessage("ID контакта должно быть больше 0.")
            .When(x => x.ContactId.HasValue);

        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(0).WithMessage("Срок выполнения не может быть отрицательным.")
            .When(x => x.DueDate.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Некорректный статус задачи.");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Некорректный приоритет задачи.");
    }
}
