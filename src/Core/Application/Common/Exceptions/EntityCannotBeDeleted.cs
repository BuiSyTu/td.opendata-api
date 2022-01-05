using System.Net;

namespace TD.OpenData.WebApi.Application.Common.Exceptions;

public class EntityCannotBeDeleted : CustomException
{
    public EntityCannotBeDeleted(string message)
    : base(message, null, HttpStatusCode.Conflict)
    {
    }
}