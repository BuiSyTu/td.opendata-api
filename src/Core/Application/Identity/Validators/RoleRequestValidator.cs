using TD.OpenData.WebApi.Application.Common.Validation;
using TD.OpenData.WebApi.Application.Identity.Interfaces;
using TD.OpenData.WebApi.Shared.DTOs.Identity;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace TD.OpenData.WebApi.Application.Identity.Validators;

public class RoleRequestValidator : CustomValidator<RoleRequest>
{
    private readonly IRoleService _roleService = default!;
    private readonly IStringLocalizer<RoleRequestValidator> _localizer;

    public RoleRequestValidator(IRoleService roleService, IStringLocalizer<RoleRequestValidator> localizer)
    {
        _roleService = roleService;
        _localizer = localizer;

        RuleFor(r => r.Name)
            .NotEmpty()
            .MustAsync(async (role, name, _) => !await _roleService.ExistsAsync(name, role.Id))
                .WithMessage(_localizer["Similar Role already exists."]);
    }
}