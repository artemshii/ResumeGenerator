using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ResumeGenerator.Data.Models.Exceptions;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            // Pass request to next middleware
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {

        HttpStatusCode statusCode;
        string message = ex.Message;

        switch (ex)
        {
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = ex.Message;
                break;

            case ValidationException:
                statusCode = HttpStatusCode.BadRequest;
                message = ex.Message;
                break;

            case UnauthorizedException:
                statusCode = HttpStatusCode.Unauthorized;
                message = ex.Message;
                break;

            case EmailException:
                statusCode = HttpStatusCode.BadRequest;
                message = ex.Message;
                break;

            case ModelStateException:
                statusCode = HttpStatusCode.BadRequest;
                message = ex.Message;
                break;

            case PaymentException:
                statusCode = HttpStatusCode.PaymentRequired;
                message = ex.Message;
                break;

            case PdfException:
                statusCode = HttpStatusCode.BadRequest;
                message = ex.Message;
                break;

            case RegisterException:
                statusCode = HttpStatusCode.BadRequest;
                message = ex.Message;
                break;

            case SessionProccesedException:
                statusCode = HttpStatusCode.Conflict;
                message = ex.Message;
                break;

            case Exception:
                statusCode = HttpStatusCode.InternalServerError;
                message = ex.Message;
                break;
            

            default:
                statusCode = HttpStatusCode.InternalServerError;
                message = "An unexpected error occurred. Please try again later.";
                break;
        }
        // Log exception
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new ResponseBase(
            ((int)statusCode),
            message  // In production, replace with generic message
            
        );

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
