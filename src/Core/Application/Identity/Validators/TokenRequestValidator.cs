using TD.OpenData.WebApi.Application.Common.Validation;
using TD.OpenData.WebApi.Shared.DTOs.Identity;
using FluentValidation;

namespace TD.OpenData.WebApi.Application.Identity.Validators;

public class TokenRequestValidator : CustomValidator<TokenRequest>
{
    public TokenRequestValidator()
    {
        RuleFor(p => p.Email).Cascade(CascadeMode.Stop).NotEmpty().EmailAddress().WithMessage("Invalid Email Address.");
        RuleFor(p => p.Password).Cascade(CascadeMode.Stop).NotEmpty();
    }
}
