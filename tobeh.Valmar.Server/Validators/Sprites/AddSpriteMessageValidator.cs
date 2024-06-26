using FluentValidation;

namespace tobeh.Valmar.Server.Validators.Sprites;

public class AddSpriteMessageValidator : AbstractValidator<AddSpriteMessage>
{
    public AddSpriteMessageValidator()
    {
        RuleFor(request => request.Cost).GreaterThan(1000).When(request => request.EventDropId is not null);
        RuleFor(request => request.Cost).GreaterThan(0);
        RuleFor(request => request.Name).NotEmpty();
        RuleFor(request => request.Url).NotEmpty();
        RuleFor(request => request.Artist).NotEmpty().When(req => req.Artist is not null);
        RuleFor(request => request.EventDropId).GreaterThan(0).When(request => request.EventDropId is not null);
    }
}