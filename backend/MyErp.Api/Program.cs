using Microsoft.EntityFrameworkCore;
using MyErp.Accounts;
using MyErp.Admin;
using MyErp.Api.Identity;
using MyErp.Api.Middleware;
using MyErp.Common.Interfaces;
using MyErp.Common.Time;
using MyErp.Data;
using MyErp.Data.Context;
using MyErp.Inventory;
using MyErp.Masters;

var builder = WebApplication.CreateBuilder(args);

// Local-only overrides (real connection strings, secrets). Gitignored, so each
// developer keeps their own and nothing sensitive reaches the repository.
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

const string CorsPolicy = "Frontend";

// ---------------------------------------------------------------------------
// Cross-cutting services
// ---------------------------------------------------------------------------
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
builder.Services.AddScoped<ICurrentUser, HttpContextCurrentUser>();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy => policy
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// ---------------------------------------------------------------------------
// Persistence: one PostgreSQL database behind one shared DbContext
// ---------------------------------------------------------------------------
builder.Services.AddPersistence(builder.Configuration);

// ---------------------------------------------------------------------------
// Business modules. Each module owns its own registration; the host only lists
// which modules are part of this deployment.
// ---------------------------------------------------------------------------
builder.Services
    .AddControllers()
    .AddMastersModule(builder.Configuration)
    .AddAdminModule(builder.Configuration)
    .AddAccountsModule(builder.Configuration)
    .AddInventoryModule(builder.Configuration);

// ---------------------------------------------------------------------------
// Authentication / authorization
// ---------------------------------------------------------------------------
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

var app = builder.Build();

// ---------------------------------------------------------------------------
// Pipeline
// ---------------------------------------------------------------------------
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors(CorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Liveness probe. Not a business feature, so it stays here in the host.
app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithName("Health");

// Readiness probe: confirms the API can actually reach PostgreSQL.
app.MapGet("/health/db", async (AppDbContext db, CancellationToken cancellationToken) =>
    await db.Database.CanConnectAsync(cancellationToken)
        ? Results.Ok(new { status = "ok", database = "reachable" })
        : Results.Problem("Database is not reachable.", statusCode: StatusCodes.Status503ServiceUnavailable))
    .WithName("HealthDatabase");

app.Run();
