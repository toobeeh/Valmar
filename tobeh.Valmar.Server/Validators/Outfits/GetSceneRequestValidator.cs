using FluentValidation;

namespace tobeh.Valmar.Server.Validators.Outfits;

public class SaveOutfitRequestValidator : AbstractValidator<SaveOutfitRequest>
{
    public SaveOutfitRequestValidator()
    {
        RuleFor(request => request.Outfit.Name).NotEmpty();
    }
}