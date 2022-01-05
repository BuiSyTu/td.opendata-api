using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Shared.DTOs.Filters;

namespace TD.OpenData.WebApi.Application.Common.Specifications;

public class Specification<T> : BaseSpecification<T>
where T : BaseEntity
{
    public string? Keyword { get; set; }
    public Search? AdvancedSearch { get; set; }
    public Filters<T>? Filters { get; set; }
}
