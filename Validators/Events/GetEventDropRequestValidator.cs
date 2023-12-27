using FluentValidation;

namespace Valmar.Validators.Events;

public class GetEventDropRequestValidator : AbstractValidator<GetEventDropRequest>
{
    public GetEventDropRequestValidator()
    {
        RuleFor(request => request.Id).GreaterThan(0);
    }
}