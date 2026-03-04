using System.Diagnostics;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// YARP reverse proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// CORS — allow React Native and web clients
builder.Services.AddCors(opts =>
{
    opts.AddDefaultPolicy(policy =>
    {
        var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                      ?? ["http://localhost:3000", "http://localhost:8081"];

        policy
            .WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Rate limiting — fixed window per IP
builder.Services.AddRateLimiter(opts =>
{
    opts.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    opts.AddFixedWindowLimiter("fixed", limiter =>
    {
        limiter.PermitLimit = 100;
        limiter.Window = TimeSpan.FromMinutes(1);
        limiter.QueueLimit = 0;
    });
    opts.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});

// Health checks — ping each downstream service
var clusters = builder.Configuration.GetSection("ReverseProxy:Clusters").GetChildren();
var healthChecks = builder.Services.AddHealthChecks();
foreach (var cluster in clusters)
{
    var address = cluster.GetSection("Destinations:destination1:Address").Value;
    if (string.IsNullOrEmpty(address)) continue;

    var name = cluster.Key; // e.g. "auth-cluster"
    healthChecks.AddUrlGroup(
        new Uri(new Uri(address), "/health"),
        name: name,
        tags: ["downstream"]);
}

var app = builder.Build();

// Request logging middleware
app.Use(async (context, next) =>
{
    var sw = Stopwatch.StartNew();
    await next();
    sw.Stop();

    var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
        .CreateLogger("RequestLog");

    logger.LogInformation(
        "{Method} {Path} → {StatusCode} in {Elapsed}ms",
        context.Request.Method,
        context.Request.Path,
        context.Response.StatusCode,
        sw.ElapsedMilliseconds);
});

app.UseCors();
app.UseRateLimiter();

app.MapHealthChecks("/health");
app.MapReverseProxy();

app.Run();
