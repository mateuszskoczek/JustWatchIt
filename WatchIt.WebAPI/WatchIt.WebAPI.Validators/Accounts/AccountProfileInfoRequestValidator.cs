using FluentValidation;
using WatchIt.Common.Model.Accounts;
using WatchIt.Database;

namespace WatchIt.WebAPI.Validators.Accounts;

public class AccountProfileInfoRequestValidator : AbstractValidator<AccountProfileInfoRequest>
{
    public AccountProfileInfoRequestValidator(DatabaseContextOld database)
    {
        RuleFor(x => x.Description).MaximumLength(1000);
        When(x => x.GenderId.HasValue, () =>
        {
            RuleFor(x => x.GenderId!.Value).MustBeIn(database.Genders.Select(x => x.Id));
        });
    }
}