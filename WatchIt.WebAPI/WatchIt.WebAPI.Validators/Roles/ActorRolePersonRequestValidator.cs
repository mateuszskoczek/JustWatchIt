using FluentValidation;
using WatchIt.Common.Model.Roles;
using WatchIt.Database;

namespace WatchIt.WebAPI.Validators.Roles;

public class ActorRolePersonRequestValidator : AbstractValidator<ActorRolePersonRequest>
{
    public ActorRolePersonRequestValidator(DatabaseContextOld database)
    {
        Include(new BaseActorRoleRequestValidator(database));
        RuleFor(x => x.MediaId).NotEmpty()
                               .NotNull()
                               .MustBeIn(database.Media.Select(x => x.Id).ToList());
    }
}