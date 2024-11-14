using FluentValidation;

namespace WatchIt.Common.Model.Genres.Genre;

public class GenreRequestValidator : AbstractValidator<GenreRequest>
{
    public GenreRequestValidator()
    {
        RuleFor(x => x.Name).MaximumLength(100);
    }
}