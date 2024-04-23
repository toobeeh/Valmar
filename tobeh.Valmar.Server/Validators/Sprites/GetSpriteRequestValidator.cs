using FluentValidation;

namespace tobeh.Valmar.Server.Validators.Sprites;

public class GetSpriteRequestValidator : AbstractValidator<GetSpriteRequest>
{
    public GetSpriteRequestValidator()
    {
        RuleFor(request => request.Id).GreaterThan(0);
    }
}