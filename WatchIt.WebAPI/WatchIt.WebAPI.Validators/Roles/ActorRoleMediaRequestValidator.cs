using FluentValidation;
using WatchIt.Common.Model.Roles;
using WatchIt.Database;

namespace WatchIt.WebAPI.Validators.Roles;

public class ActorRoleMediaRequestValidator : AbstractValidator<ActorRoleMediaRequest>
{
    public ActorRoleMediaRequestValidator(DatabaseContextOld database)
    {
        Include(new BaseActorRoleRequestValidator(database));
        RuleFor(x => x.PersonId).NotEmpty()
                                .NotNull()
                                .MustBeIn(database.Persons.Select(x => x.Id).ToList());
    }
}