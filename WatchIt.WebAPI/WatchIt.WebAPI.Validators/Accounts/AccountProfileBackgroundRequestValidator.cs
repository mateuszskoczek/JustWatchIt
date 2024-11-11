using FluentValidation;
using WatchIt.Common.Model.Accounts;
using WatchIt.Database;

namespace WatchIt.WebAPI.Validators.Accounts;

public class AccountProfileBackgroundRequestValidator : AbstractValidator<AccountProfileBackgroundRequest>
{
    public AccountProfileBackgroundRequestValidator(DatabaseContextOld database)
    {
        RuleFor(x => x.Id).MustBeIn(database.MediaPhotoImages.Where(x => x.MediaPhotoImageBackground != null), x => x.Id)
                          .WithMessage("Image has to be background");
    }
}