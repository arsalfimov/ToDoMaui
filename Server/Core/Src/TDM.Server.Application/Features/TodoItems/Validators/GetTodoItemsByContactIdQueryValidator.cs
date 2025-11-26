using FluentValidation;
using TDM.Server.Application.Features.TodoItems.Queries;

namespace TDM.Server.Application.Features.TodoItems.Validators;

public class GetTodoItemsByContactIdQueryValidator : AbstractValidator<GetTodoItemsByContactIdQuery>
{
    public GetTodoItemsByContactIdQueryValidator()
    {
        RuleFor(x => x.ContactId)
            .GreaterThan(0).WithMessage("ID контакта должно быть больше 0.");
    }
}
