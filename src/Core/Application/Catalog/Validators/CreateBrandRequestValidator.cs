using TD.OpenData.WebApi.Application.Common.Validation;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using FluentValidation;

namespace TD.OpenData.WebApi.Application.Catalog.Validators;

public class CreateBrandRequestValidator : CustomValidator<CreateBrandRequest>
{
    public CreateBrandRequestValidator()
    {
        RuleFor(p => p.Name).MaximumLength(75).NotEmpty();
    }
}