using FluentValidation;

namespace tobeh.Valmar.Server.Validators.Awards;

public class GetAwardRequestValidator : AbstractValidator<GetAwardRequest>
{
    public GetAwardRequestValidator()
    {
        RuleFor(request => request.Id).GreaterThan(0);
    }
}