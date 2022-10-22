using CTeleportAssignment.Services.Exceptions;
using CTeleportAssignment.WebAPI.Models;
using System.Net;
using System.Text.Json;

namespace CTeleportAssignment.WebAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went wrong while processing {context.Request.Path}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            ApiErrorDetail apiErrorDetail = new ApiErrorDetail();
            switch (ex)
            {
                case IataNotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    apiErrorDetail.ErrorType = "Not Found";
                    apiErrorDetail.Message = notFoundException.Message;
                    apiErrorDetail.StatusCode = (int)statusCode;
                    break;
                case InvalidIataProvidedException invalidIataProvidedException:
                    statusCode = HttpStatusCode.BadRequest;
                    apiErrorDetail.ErrorType = "Bad Request";
                    apiErrorDetail.Message = invalidIataProvidedException.Message;
                    apiErrorDetail.StatusCode = (int)statusCode;
                    break;
                case ServiceException serviceException:
                    statusCode = HttpStatusCode.InternalServerError;
                    apiErrorDetail.ErrorType = "Service Failed";
                    apiErrorDetail.Message = serviceException.Message;
                    apiErrorDetail.StatusCode = (int)statusCode;
                    break;
                default:
                    break;
            }
            ApiResponse<Object> apiResponse = new ApiResponse<Object>("")
            {
                ErrorDetail = apiErrorDetail
            };
            string response = JsonSerializer.Serialize(apiResponse);
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(response);
        }
    }
}
