namespace TD.OpenData.WebApi.Domain.Contracts;

public interface IMustHaveTenant
{
    public string? Tenant { get; set; }
}