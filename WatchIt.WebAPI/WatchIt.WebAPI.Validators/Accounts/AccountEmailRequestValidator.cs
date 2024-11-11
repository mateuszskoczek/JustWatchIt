using FluentValidation;
using WatchIt.Common.Model.Accounts;
using WatchIt.Database;

namespace WatchIt.WebAPI.Validators.Accounts;

public class AccountEmailRequestValidator : AbstractValidator<AccountEmailRequest>
{
    public AccountEmailRequestValidator(DatabaseContextOld database)
    {
        RuleFor(x => x.NewEmail).EmailAddress()
                                .CannotBeIn(database.Accounts, x => x.Email)
                                .WithMessage("Email was already used");
    }
}