using FluentValidation;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using WatchIt.Common.Model.Media;
using WatchIt.WebAPI.Validators.Photos;

namespace WatchIt.WebAPI.Validators.Media;

public class MediaPhotoRequestValidator : AbstractValidator<MediaPhotoRequest>
{
    public MediaPhotoRequestValidator()
    {
        RuleFor(x => x.Image).NotEmpty();
        RuleFor(x => x.MimeType).Matches(@"\w+/.+").WithMessage("Incorrect mimetype");
        When(x => x.Background is not null, () =>
        {
            RuleFor(x => x.Background!).SetValidator(new PhotoBackgroundDataValidator());
        });
    }
}