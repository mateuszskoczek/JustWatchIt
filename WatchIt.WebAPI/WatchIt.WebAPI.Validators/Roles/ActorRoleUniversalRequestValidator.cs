using FluentValidation;
using WatchIt.Common.Model.Roles;
using WatchIt.Database;

namespace WatchIt.WebAPI.Validators.Roles;

public class ActorRoleUniversalRequestValidator : AbstractValidator<ActorRoleUniversalRequest>
{
    public ActorRoleUniversalRequestValidator(DatabaseContextOld database)
    {
        Include(new BaseActorRoleRequestValidator(database));
        RuleFor(x => x.PersonId).NotEmpty()
                                .NotNull()
                                .MustBeIn(database.Persons.Select(x => x.Id).ToList());
        RuleFor(x => x.MediaId).NotEmpty()
                               .NotNull()
                               .MustBeIn(database.Media.Select(x => x.Id).ToList());
    }
}