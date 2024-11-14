using FluentValidation;

namespace WatchIt.Common.Model.Genders.Gender;

public class GenderRequestValidator : AbstractValidator<GenderRequest>
{
    public GenderRequestValidator()
    {
        RuleFor(x => x.Name).MaximumLength(100);
    }
}