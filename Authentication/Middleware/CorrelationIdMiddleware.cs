using Serilog.Context;

namespace Authentication.Middleware
{
    public class CorrelationIdMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();

            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }

            // Aggiungo il Correlation ID all'header della richiesta
            context.Request.Headers["X-Correlation-ID"] = correlationId;

            // Aggiungo il Correlation ID all'header della risposta
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Append("X-Correlation-ID", correlationId);
                return Task.CompletedTask;
            });

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }

}