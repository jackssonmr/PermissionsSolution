namespace Permissions.API.Middleware;

public class CsrfMiddleware
{
    private readonly RequestDelegate _next;

    public CsrfMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put ||
            context.Request.Method == HttpMethods.Patch || context.Request.Method == HttpMethods.Delete)
        {
            // Verificar si el token CSRF está presente en la solicitud
            string csrfToken = context.Request.Headers["X-CSRF-TOKEN"];

            // Comprobar si el token CSRF coincide con el token generado para el usuario actual
            if (string.IsNullOrWhiteSpace(csrfToken) || !csrfToken.Equals(context.User.FindFirst("CSRF-TOKEN")?.Value))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }
        }

        // Llamar al siguiente middleware en la cadena de middleware
        await _next(context);
    }
}