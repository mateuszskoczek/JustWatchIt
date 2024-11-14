using FluentValidation;

namespace WatchIt.Common.Model.Accounts.AccountAuthenticate;

public class AccountAuthenticateRequestValidator : AbstractValidator<AccountAuthenticateRequest>
{
    public AccountAuthenticateRequestValidator()
    {
        RuleFor(x => x.UsernameOrEmail).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}