using FluentValidation;
using Valmar.Domain;

namespace Valmar.Validators.Themes;

public class ShareThemeRequestValidator : AbstractValidator<ShareThemeRequest>
{
    private readonly IThemesDomainService _themesService;
    
    public ShareThemeRequestValidator(IThemesDomainService themesService)
    {
        this._themesService = themesService;

        RuleFor(request => request.ThemeJson).Must(BeValidThemeJson).WithMessage("Invalid theme json");
    }

    private bool BeValidThemeJson(string json)
    {
        try
        {
            var meta = _themesService.ParseTheme(json);
            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }
}