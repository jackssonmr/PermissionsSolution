using System.Text;
using Serilog;

namespace Permissions.API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Log the request details
            var requestDetails = await FormatRequest(context.Request);
            _logger.LogInformation(requestDetails);
            
            // Capture the request body for logging
            if (context.Request.Method.Equals("POST") || context.Request.Method.Equals("PUT"))
            {
                context.Request.EnableBuffering();
                var requestBodyStream = new MemoryStream();
                await context.Request.Body.CopyToAsync(requestBodyStream);
                requestBodyStream.Seek(0, SeekOrigin.Begin);
                var requestBodyText = new StreamReader(requestBodyStream, Encoding.UTF8).ReadToEnd();
                Log.Information($"Request body: {requestBodyText}");
                context.Request.Body.Seek(0, SeekOrigin.Begin);
            }
            
            // Call the next middleware in the pipeline
            await _next(context);
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            // Log request method, path and query string
            var requestDetails = new StringBuilder();
            requestDetails.AppendLine($"Request Method: {request.Method}");
            requestDetails.AppendLine($"Request Path: {request.Path}");
            requestDetails.AppendLine($"Request Query String: {request.QueryString}");

            // Log request headers
            requestDetails.AppendLine("Request Headers:");
            foreach (var header in request.Headers)
            {
                requestDetails.AppendLine($"{header.Key}: {header.Value}");
            }

            // Log request body (if present)
            if (request.ContentLength.HasValue && request.ContentLength > 0 && request.Body.CanSeek)
            {
                request.Body.Seek(0, SeekOrigin.Begin);
                var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
                requestDetails.AppendLine("Request Body:");
                requestDetails.AppendLine(requestBody);
            }

            return requestDetails.ToString();
        }
    }
}
