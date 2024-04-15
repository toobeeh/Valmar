using FluentValidation;

namespace Valmar.Validators.Outfits;

public class SaveOutfitRequestValidator : AbstractValidator<SaveOutfitRequest>
{
    public SaveOutfitRequestValidator()
    {
        RuleFor(request => request.Outfit.Name).NotEmpty();
    }
}