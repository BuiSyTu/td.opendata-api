using TD.OpenData.WebApi.Application.Common.Validation;
using TD.OpenData.WebApi.Application.FileStorage;
using TD.OpenData.WebApi.Shared.DTOs.Identity;
using FluentValidation;

namespace TD.OpenData.WebApi.Application.Identity.Validators;

public class UpdateProfileRequestValidator : CustomValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(p => p.FirstName).MaximumLength(75).NotEmpty();
        RuleFor(p => p.LastName).MaximumLength(75).NotEmpty();
        RuleFor(p => p.Email).NotEmpty();
        RuleFor(p => p.Image).SetNonNullableValidator(new FileUploadRequestValidator());
    }
}