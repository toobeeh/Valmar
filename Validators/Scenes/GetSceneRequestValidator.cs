using FluentValidation;

namespace Valmar.Validators.Scenes;

public class GetSceneRequestValidator : AbstractValidator<GetSceneRequest>
{
    public GetSceneRequestValidator()
    {
        RuleFor(request => request.Id).GreaterThan(0);
    }
}