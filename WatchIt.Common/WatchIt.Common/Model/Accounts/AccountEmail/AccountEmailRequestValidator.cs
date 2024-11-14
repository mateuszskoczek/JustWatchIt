using FluentValidation;
using WatchIt.Common.Model.Accounts;
using WatchIt.Common.Validation;
using WatchIt.Database;

namespace WatchIt.Common.Model.Accounts.AccountEmail;

public class AccountEmailRequestValidator : AbstractValidator<AccountEmailRequest>
{
    public AccountEmailRequestValidator(DatabaseContext database)
    {
        RuleFor(x => x.NewEmail).EmailAddress()
                                .CannotBeIn(database.Accounts, x => x.Email)
                                .WithMessage("Email was already used");
    }
}