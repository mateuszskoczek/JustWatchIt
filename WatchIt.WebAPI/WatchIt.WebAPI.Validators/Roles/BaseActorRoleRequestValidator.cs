using FluentValidation;
using WatchIt.Common.Model.Roles;
using WatchIt.Database;

namespace WatchIt.WebAPI.Validators.Roles;

public class BaseActorRoleRequestValidator : AbstractValidator<ActorRoleRequest>
{
    public BaseActorRoleRequestValidator(DatabaseContextOld database)
    {
        RuleFor(x => x.Name).NotEmpty()
                            .MaximumLength(100);
        RuleFor(x => x.TypeId).NotEmpty()
                              .NotNull()
                              .MustBeIn(database.PersonActorRoleTypes.Select(x => x.Id).ToList());
    }
}