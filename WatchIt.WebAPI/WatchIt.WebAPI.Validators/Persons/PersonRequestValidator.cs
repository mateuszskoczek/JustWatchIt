using FluentValidation;
using WatchIt.Common.Model.Persons;
using WatchIt.Database;

namespace WatchIt.WebAPI.Validators.Persons;

public class PersonRequestValidator : AbstractValidator<PersonRequest>
{
    public PersonRequestValidator(DatabaseContextOld database)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.FullName).MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000);
        When(x => x.GenderId.HasValue, () =>
        {
            RuleFor(x => x.GenderId!.Value).MustBeIn(database.Genders.Select(g => g.Id));
        });
    }
}