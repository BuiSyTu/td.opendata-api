using TD.OpenData.WebApi.Application.Common.Validation;
using TD.OpenData.WebApi.Application.FileStorage;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using FluentValidation;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.Validators;

public class CreateProductRequestValidator : CustomValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(p => p.Name).MaximumLength(75).NotEmpty();
        RuleFor(p => p.Rate).GreaterThanOrEqualTo(1).NotEqual(0);
        RuleFor(p => p.Image).SetNonNullableValidator(new FileUploadRequestValidator());
        RuleFor(p => p.BrandId).NotEmpty().NotNull();
    }
}