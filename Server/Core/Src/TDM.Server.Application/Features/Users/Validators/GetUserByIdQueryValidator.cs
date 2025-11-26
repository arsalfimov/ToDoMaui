using FluentValidation;
using TDM.Server.Application.Features.Users.Queries;

namespace TDM.Server.Application.Features.Users.Validators;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id должен быть больше 0.");
    }
}
