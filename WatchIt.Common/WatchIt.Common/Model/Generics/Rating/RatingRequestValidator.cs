using FluentValidation;

namespace WatchIt.Common.Model.Generics.Rating;

public class RatingRequestValidator : AbstractValidator<RatingRequest>
{
    public RatingRequestValidator()
    {
        RuleFor(x => x.Rating).InclusiveBetween((byte)1, (byte)10);
    }
}