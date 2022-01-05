using System.Net;

namespace TD.OpenData.WebApi.Application.Common.Exceptions;

public class EntityAlreadyExistsException : CustomException
{
    public EntityAlreadyExistsException(string message)
    : base(message, null, HttpStatusCode.Conflict)
    {
    }
}