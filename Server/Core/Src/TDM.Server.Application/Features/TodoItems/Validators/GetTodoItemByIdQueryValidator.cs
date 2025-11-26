using FluentValidation;
using TDM.Server.Application.Features.TodoItems.Queries;

namespace TDM.Server.Application.Features.TodoItems.Validators;

public class GetTodoItemByIdQueryValidator : AbstractValidator<GetTodoItemByIdQuery>
{
    public GetTodoItemByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID задачи должно быть больше 0.");
    }
}
