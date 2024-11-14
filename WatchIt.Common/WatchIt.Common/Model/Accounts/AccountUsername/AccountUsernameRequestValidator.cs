using FluentValidation;
using WatchIt.Common.Model.Accounts;
using WatchIt.Common.Validation;
using WatchIt.Database;

namespace WatchIt.Common.Model.Accounts.AccountUsername;

public class AccountUsernameRequestValidator : AbstractValidator<AccountUsernameRequest>
{
    public AccountUsernameRequestValidator(DatabaseContext database)
    {
        RuleFor(x => x.NewUsername).MinimumLength(5)
                                   .MaximumLength(50)
                                   .CannotBeIn(database.Accounts, x => x.Username)
                                   .WithMessage("Username is already used");
    }
}