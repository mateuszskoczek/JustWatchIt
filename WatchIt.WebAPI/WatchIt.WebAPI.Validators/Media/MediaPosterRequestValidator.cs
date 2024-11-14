using FluentValidation;
using WatchIt.Common.Model.Media;

namespace WatchIt.WebAPI.Validators.Media;

public class MediaPosterRequestValidator : AbstractValidator<MediaPosterRequest>
{
    public MediaPosterRequestValidator()
    {
        RuleFor(x => x.Image).NotEmpty();
        RuleFor(x => x.MimeType).Matches(@"\w+/.+").WithMessage("Incorrect mimetype");
    }
}