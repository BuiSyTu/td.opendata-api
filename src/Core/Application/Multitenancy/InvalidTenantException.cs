using System.Net;
using TD.OpenData.WebApi.Application.Common.Exceptions;

namespace TD.OpenData.WebApi.Application.Multitenancy;

public class InvalidTenantException : CustomException
{
    public InvalidTenantException(string message)
    : base(message, null, HttpStatusCode.Unauthorized)
    {
    }
}