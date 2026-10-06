using System.Net;
using System.Text.Json;

namespace Pokedex.Middlewares
{
    // ============================================================
    // GLOBAL EXCEPTION HANDLER MIDDLEWARE
    //
    // BAKIT KAILANGAN?
    // - Kapag may naganap na error kahit saan sa system,
    //   dito ito nahuhuli at nire-return ng maayos na JSON response
    // - Walang nakakatakot na error messages na lalabas sa user
    // - Lahat ng unhandled exceptions ay mahuhuli dito
    //
    // OOP CONCEPT: ENCAPSULATION
    // - Ang error handling logic ay naka-encapsulate sa isang class
    // - Hindi kailangan ng Controller na mag-try-catch ng lahat
    //
    // PAANO GUMAGANA:
    // User Request → [GlobalExceptionMiddleware] → Controller → Service → Repository → DB
    //                          ↑
    //               Kapag may exception kahit saan,
    //               babalik dito at mahuhuli ang error
    // ============================================================
    public class GlobalExceptionHandlerMiddleware
    {
        // _next — ang susunod na middleware sa pipeline
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // InvokeAsync — ito ang tinatawag ng ASP.NET bawat HTTP request
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Ipasa ang request sa susunod na middleware/controller
                await _next(context);
            }
            catch (Exception ex)
            {
                // Nahuli ang exception! I-log at i-handle ito
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        // I-convert ang exception sa JSON error response
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // I-determine ang tamang status code base sa exception type
            var (statusCode, message) = exception switch
            {
                ArgumentNullException => (HttpStatusCode.BadRequest, "Required data is missing."),
                ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
                KeyNotFoundException => (HttpStatusCode.NotFound, "The requested Pokemon was not found."),
                InvalidOperationException => (HttpStatusCode.Conflict, exception.Message),
                // Default: lahat ng iba pang exceptions = 500 Internal Server Error
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again.")
            };

            context.Response.StatusCode = (int)statusCode;

            // Gumawa ng structured error response object
            var errorResponse = new
            {
                statusCode = (int)statusCode,
                message = message,
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            };

            // I-serialize at i-write ang JSON response
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(errorResponse, jsonOptions);
            await context.Response.WriteAsync(json);
        }
    }

    // ============================================================
    // EXTENSION METHOD — Para mas madaling gamitin sa Program.cs
    // Imbes na: app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    // Pwede na: app.UseGlobalExceptionHandler();
    //
    // OOP CONCEPT: EXTENSION METHOD — nagdadagdag ng method
    // sa existing class (IApplicationBuilder) nang hindi binabago ito
    // ============================================================
    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(
            this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}