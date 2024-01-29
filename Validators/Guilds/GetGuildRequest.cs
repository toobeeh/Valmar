using FluentValidation;

namespace Valmar.Validators.Guilds;

public class GetGuildRequestValidator : AbstractValidator<GetGuildRequest>
{
    public GetGuildRequestValidator()
    {
        RuleFor(request => request.ObserveToken).GreaterThan(0);
    }
}