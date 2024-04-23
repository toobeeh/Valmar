using FluentValidation;

namespace tobeh.Valmar.Server.Validators.Events;

public class GetEventRequestValidator : AbstractValidator<GetEventRequest>
{
    public GetEventRequestValidator()
    {
        RuleFor(request => request.Id).GreaterThan(0);
    }
}