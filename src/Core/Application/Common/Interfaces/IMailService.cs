using TD.OpenData.WebApi.Shared.DTOs.Mailing;

namespace TD.OpenData.WebApi.Application.Common.Interfaces;

public interface IMailService : ITransientService
{
    Task SendAsync(MailRequest request);
}