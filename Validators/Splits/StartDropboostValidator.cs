using FluentValidation;

namespace Valmar.Validators.Splits;

public class StartDropboostValidator : AbstractValidator<StartDropboostRequest>
{
    public StartDropboostValidator()
    {
        RuleFor(request => request.CooldownSplits).GreaterThanOrEqualTo(0);
        RuleFor(request => request.FactorSplits).GreaterThanOrEqualTo(0);
        RuleFor(request => request.DurationSplits).GreaterThanOrEqualTo(0);
    }
}