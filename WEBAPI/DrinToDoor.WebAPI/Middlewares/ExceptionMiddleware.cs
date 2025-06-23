using DrinkToDoor.BLL.Exceptions;
using DrinkToDoor.BLL.ViewModel.ApiResponse;
using DrinToDoor.WebAPI.Extensions;
using System.Text.Json;

namespace DrinToDoor.WebAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    await HandleAppException(context, new AppException(ErrorCode.UNAUTHORIZED));
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    await HandleAppException(context, new AppException(ErrorCode.FORBIDDEN));
                }
            }
            catch (AppException ex)
            {
                await HandleAppException(context, ex);
            }
            catch (UnauthorizedAccessException)
            {
                await HandleAppException(context, new AppException(ErrorCode.UNAUTHORIZED));
            }
            catch (Exception ex)
            {
                await HandleGenericException(context, ex);
            }
        }

        private async Task HandleAppException(HttpContext context, AppException ex)
        {
            _logger.LogError(ex, "An application exception occurred: {ErrorCode}", ex.ErrorCode);

            var status = ex.ErrorCode.GetStatusCode();
            var message = ex.ErrorCode.GetMessage(ex.Message);

            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/json";

            var response = new ApiResponse
            {
                Code = (int)status,
                Success = false,
                Message = message,
                Data = null
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }

        private async Task HandleGenericException(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            context.Response.ContentType = "application/json";
            var response = new ApiResponse
            {
                Code = StatusCodes.Status500InternalServerError,
                Success = false,
                Message = _env.IsDevelopment()
                    ? $"Internal Server Error: {ex.Message}"
                    : "An unexpected error occurred. Please try again later.",
                Data = _env.IsDevelopment()
                    ? new { Exception = ex.GetType().Name, Message = ex.Message, StackTrace = ex.StackTrace }
                    : null
            };
            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            await context.Response.WriteAsync(json);
        }

        private async Task HandleUnauthorizedResponse(HttpContext context)
        {
            await HandleAppException(context, new AppException(ErrorCode.UNAUTHORIZED));
        }

        private async Task HandleForbiddenResponse(HttpContext context)
        {
            await HandleAppException(context, new AppException(ErrorCode.FORBIDDEN));
        }
    }
}
