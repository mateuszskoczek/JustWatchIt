using FluentValidation;
using WatchIt.Common.Model.Accounts;
using WatchIt.Database;

namespace WatchIt.WebAPI.Validators.Accounts;

public class AccountUsernameRequestValidator : AbstractValidator<AccountUsernameRequest>
{
    public AccountUsernameRequestValidator(DatabaseContextOld database)
    {
        RuleFor(x => x.NewUsername).MinimumLength(5)
                                   .MaximumLength(50)
                                   .CannotBeIn(database.Accounts, x => x.Username)
                                   .WithMessage("Username is already used");
    }
}