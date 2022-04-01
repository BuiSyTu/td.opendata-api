using TD.OpenData.WebApi.Application.Common.Validation;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using FluentValidation;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.Validators;

public class UpdateBrandRequestValidator : CustomValidator<UpdateBrandRequest>
{
    public UpdateBrandRequestValidator()
    {
        RuleFor(p => p.Name).MaximumLength(75).NotEmpty();
    }
}