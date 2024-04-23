using FluentValidation;

namespace tobeh.Valmar.Server.Validators.Splits;

public class UpgradeDropboostValidator : AbstractValidator<UpgradeDropboostRequest>
{
    public UpgradeDropboostValidator()
    {
        RuleFor(request => request.CooldownSplitsIncrease).GreaterThanOrEqualTo(0);
        RuleFor(request => request.DurationSplitsIncrease).GreaterThanOrEqualTo(0);
        RuleFor(request => request.FactorSplitsIncrease).GreaterThanOrEqualTo(0);
    }
}