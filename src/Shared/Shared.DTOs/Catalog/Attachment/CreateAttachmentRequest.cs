using Microsoft.AspNetCore.Http;

namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class CreateAttachmentRequest : IMustBeValid
{
    public List<IFormFile>? Files { get; set; }

}