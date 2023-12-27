using FluentValidation;

namespace Valmar.Validators.Sprites;

public class GetSpriteRequestValidator : AbstractValidator<GetSpriteRequest>
{
    public GetSpriteRequestValidator()
    {
        RuleFor(request => request.Id).GreaterThan(0);
    }
}