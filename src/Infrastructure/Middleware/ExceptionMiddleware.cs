using System.Net;
using TD.OpenData.WebApi.Application.Common.Exceptions;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Identity.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Serilog;
using Serilog.Context;

namespace TD.OpenData.WebApi.Infrastructure.Middleware;

internal class ExceptionMiddleware : IMiddleware
{
    private readonly ICurrentUser _currentUser;
    private readonly ISerializerService _jsonSerializer;
    private readonly IStringLocalizer<ExceptionMiddleware> _localizer;

    public ExceptionMiddleware(
        ISerializerService jsonSerializer,
        ICurrentUser currentUser,
        IStringLocalizer<ExceptionMiddleware> localizer)
    {
        _jsonSerializer = jsonSerializer;
        _currentUser = currentUser;
        _localizer = localizer;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            string email = _currentUser.GetUserEmail() is string userEmail ? userEmail : "Anonymous";
            string? userId = _currentUser.GetUserName();
            //string tenant = _currentUser.GetTenant() ?? string.Empty;
            string tenant = _currentUser.GetTenant() ?? "root";
            if (userId != string.Empty) LogContext.PushProperty("UserName", userId);
            LogContext.PushProperty("UserEmail", email);
            if (!string.IsNullOrEmpty(tenant)) LogContext.PushProperty("Tenant", tenant);
            string errorId = Guid.NewGuid().ToString();
            LogContext.PushProperty("ErrorId", errorId);
            LogContext.PushProperty("StackTrace", exception.StackTrace);
            var errorResult = new ErrorResult
            {
                Source = exception.TargetSite?.DeclaringType?.FullName,
                Exception = exception.Message.Trim(),
                ErrorId = errorId,
                SupportMessage = _localizer["exceptionmiddleware.supportmessage"]
            };
            errorResult.Messages!.Add(exception.Message);
            var response = context.Response;
            response.ContentType = "application/json";
            if (exception is not CustomException && exception.InnerException != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }

            switch (exception)
            {
                case CustomException e:
                    response.StatusCode = errorResult.StatusCode = (int)e.StatusCode;
                    if (e.ErrorMessages is not null)
                    {
                        errorResult.Messages = e.ErrorMessages;
                    }

                    break;

                case KeyNotFoundException:
                    response.StatusCode = errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    response.StatusCode = errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            Log.Error($"{errorResult.Exception} Request failed with Status Code {context.Response.StatusCode} and Error Id {errorId}.");
            await response.WriteAsync(_jsonSerializer.Serialize(errorResult));
        }
    }
}