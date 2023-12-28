using FluentValidation;

namespace Valmar.Validators.Themes;

public class UpdateThemeRequestValidator : AbstractValidator<UpdateThemeRequest>
{
    public UpdateThemeRequestValidator()
    {
        RuleFor(request => request.NewId).NotEqual(request => request.Id).WithMessage("Update id must be different");
    }
}