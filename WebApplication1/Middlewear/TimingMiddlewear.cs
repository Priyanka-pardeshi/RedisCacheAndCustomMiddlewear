namespace WebApplication1.Middlewear;
public class TimingMiddlewear
{

    private readonly ILogger<TimingMiddlewear> _logger;
    private readonly RequestDelegate _next;
    //Next is to call next middlewear
    public TimingMiddlewear(ILogger<TimingMiddlewear> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

     public async Task Invoke(HttpContext ctx)
    {

        var start = DateTime.UtcNow;
        await _next.Invoke(ctx);
        _logger.LogInformation($"Timing: {ctx.Request.Path}:{(DateTime.UtcNow - start).TotalMilliseconds}ms");
    }
  
}

//beacuse of this we can declare app.UseTiming() in progam.cs file
//other wise use app.UseMiddleware<TimingMiddlewear>();
public static class TimingExtenstions
{
    public static IApplicationBuilder UseTiming(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TimingMiddlewear>();
    }
}
