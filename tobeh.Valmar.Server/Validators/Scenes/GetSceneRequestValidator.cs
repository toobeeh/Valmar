using FluentValidation;

namespace tobeh.Valmar.Server.Validators.Scenes;

public class GetSceneRequestValidator : AbstractValidator<GetSceneRequest>
{
    public GetSceneRequestValidator()
    {
        RuleFor(request => request.Id).GreaterThan(0);
    }
}