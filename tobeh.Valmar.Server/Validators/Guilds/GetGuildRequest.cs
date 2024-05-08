using FluentValidation;

namespace tobeh.Valmar.Server.Validators.Guilds;

public class GetGuildRequestValidator : AbstractValidator<GetGuildRequest>
{
    public GetGuildRequestValidator()
    {
        RuleFor(request => request.Invite).GreaterThan(0);
    }
}