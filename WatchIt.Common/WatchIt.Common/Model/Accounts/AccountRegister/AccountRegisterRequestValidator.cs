using FluentValidation;
using WatchIt.Common.Validation;
using WatchIt.Database;

namespace WatchIt.Common.Model.Accounts.AccountRegister;

public class AccountRegisterRequestValidator : AbstractValidator<AccountRegisterRequest>
{
    public AccountRegisterRequestValidator(DatabaseContext database)
    {
        RuleFor(x => x.Username).MinimumLength(5)
                                .MaximumLength(50)
                                .CannotBeIn(database.Accounts, x => x.Username).WithMessage("Username was already used");
        RuleFor(x => x.Email).EmailAddress()
                             .CannotBeIn(database.Accounts, x => x.Email).WithMessage("Email was already used");
        RuleFor(x => x.Password).MinimumLength(8)
                                .Must(x => x.Any(char.IsUpper)).WithMessage("Password must contain at least one uppercase letter.")
                                .Must(x => x.Any(char.IsLower)).WithMessage("Password must contain at least one lowercase letter.")
                                .Must(x => x.Any(char.IsDigit)).WithMessage("Password must contain at least one digit.");
    }
}