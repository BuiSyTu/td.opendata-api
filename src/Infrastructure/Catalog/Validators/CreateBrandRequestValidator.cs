using TD.OpenData.WebApi.Application.Common.Validation;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using FluentValidation;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.Validators;

public class CreateBrandRequestValidator : CustomValidator<CreateBrandRequest>
{
    public CreateBrandRequestValidator()
    {
        RuleFor(p => p.Name).MaximumLength(75).NotEmpty();
    }
}