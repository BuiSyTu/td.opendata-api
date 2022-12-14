using System.Net;
using TD.OpenData.WebApi.Application.Common.Exceptions;

namespace TD.OpenData.WebApi.Application.Identity.Exceptions;

public class IdentityException : CustomException
{
    public IdentityException(string message, List<string>? errors = default, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message, errors, statusCode)
    {
    }
}