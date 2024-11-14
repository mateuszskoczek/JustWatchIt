using FluentValidation;
using WatchIt.Common.Validation;
using WatchIt.Database;

namespace WatchIt.Common.Model.Accounts.AccountBackgroundPicture;

public class AccountBackgroundPictureRequestValidator : AbstractValidator<AccountBackgroundPictureRequest>
{
    public AccountBackgroundPictureRequestValidator(DatabaseContext database)
    {
        RuleFor(x => x.Id).MustBeIn(database.PhotoBackgrounds, x => x.Id)
                          .WithMessage("Background does not exists");
    }
}