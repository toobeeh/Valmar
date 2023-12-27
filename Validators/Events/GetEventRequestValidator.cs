using FluentValidation;

namespace Valmar.Validators.Events;

public class GetEventRequestValidator : AbstractValidator<GetEventRequest>
{
    public GetEventRequestValidator()
    {
        RuleFor(request => request.Id).GreaterThan(0);
    }
}