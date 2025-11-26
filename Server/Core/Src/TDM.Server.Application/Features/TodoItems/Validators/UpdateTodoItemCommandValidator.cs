using FluentValidation;
using TDM.Server.Application.Features.TodoItems.Commands;

namespace TDM.Server.Application.Features.TodoItems.Validators;

public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID задачи должно быть больше 0.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Заголовок задачи обязателен.")
            .MinimumLength(1).WithMessage("Заголовок должен содержать минимум 1 символ.")
            .MaximumLength(200).WithMessage("Заголовок не должен превышать 200 символов.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Некорректное значение статуса.");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Некорректное значение приоритета.");

        RuleFor(x => x.ContactId)
            .GreaterThan(0).WithMessage("ID контакта должно быть больше 0.")
            .When(x => x.ContactId.HasValue);

        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(0).WithMessage("Дата должна быть корректной.")
            .When(x => x.DueDate.HasValue);
    }
}
