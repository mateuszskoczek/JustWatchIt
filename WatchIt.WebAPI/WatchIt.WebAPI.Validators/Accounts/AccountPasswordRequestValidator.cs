using FluentValidation;
using WatchIt.Common.Model.Accounts;
using WatchIt.Database;

namespace WatchIt.WebAPI.Validators.Accounts;

public class AccountPasswordRequestValidator : AbstractValidator<AccountPasswordRequest>
{
    public AccountPasswordRequestValidator(DatabaseContextOld database)
    {
        RuleFor(x => x.NewPassword).MinimumLength(8)
                                   .Must(x => x.Any(char.IsUpper)).WithMessage("Password must contain at least one uppercase letter.")
                                   .Must(x => x.Any(char.IsLower)).WithMessage("Password must contain at least one lowercase letter.")
                                   .Must(x => x.Any(char.IsDigit)).WithMessage("Password must contain at least one digit.")
                                   .Equal(x => x.NewPasswordConfirmation);
    }
}