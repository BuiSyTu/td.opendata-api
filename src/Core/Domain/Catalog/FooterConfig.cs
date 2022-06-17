using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class FooterConfig : AuditableEntity
{
    public string? SoftwareName { get; set; }
    public string? CompanyName { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Fax { get; set; }
    public string? HotLine { get; set; }
    public string? Email { get; set; }


    public FooterConfig Update(UpdateFooterConfigRequest request)
    {
        if (request.SoftwareName != null && !SoftwareName.NullToString().Equals(request.SoftwareName)) SoftwareName = request.SoftwareName;
        if (request.CompanyName != null && !CompanyName.NullToString().Equals(request.CompanyName)) CompanyName = request.CompanyName;
        if (request.Address != null && !Address.NullToString().Equals(request.Address)) Address = request.Address;
        if (request.PhoneNumber != null && !PhoneNumber.NullToString().Equals(request.PhoneNumber)) PhoneNumber = request.PhoneNumber;
        if (request.Fax != null && !Fax.NullToString().Equals(request.Fax)) Fax = request.Fax;
        if (request.HotLine != null && !HotLine.NullToString().Equals(request.HotLine)) HotLine = request.HotLine;
        if (request.Email != null && !Email.NullToString().Equals(request.Email)) Email = request.Email;
        return this;
    }
}