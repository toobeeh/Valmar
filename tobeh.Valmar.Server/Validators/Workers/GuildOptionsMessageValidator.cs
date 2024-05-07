using FluentValidation;

namespace tobeh.Valmar.Server.Validators.Workers;

public class GuildOptionsMessageValidator : AbstractValidator<GuildOptionsMessage>
{
    public GuildOptionsMessageValidator()
    {
        RuleFor(request => request.Name).NotEmpty();
        RuleFor(request => request.Prefix).NotEmpty();
    }
}