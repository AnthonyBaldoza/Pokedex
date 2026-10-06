using Microsoft.EntityFrameworkCore;
using Pokedex.Data;
using Pokedex.Middlewares;
using Pokedex.Repositories;
using Pokedex.Services;

// ============================================================
// PROGRAM.CS — Entry point ng application
// Dito nag-start ang lahat
//
// May dalawang bahagi:
// 1. BUILDER PHASE — I-register ang lahat ng services sa DI container
// 2. APP PHASE — I-configure ang middleware pipeline
// ============================================================

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// PHASE 1: REGISTER SERVICES (Dependency Injection Container)
// ============================================================

// I-register ang Controllers
builder.Services.AddControllers();

// Swagger — para sa API documentation at testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQL Server Database — kinukuha ang connection string mula sa appsettings
builder.Services.AddDbContext<PokedexDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ============================================================
// DEPENDENCY INJECTION REGISTRATIONS
//
// AddScoped = bagong instance per HTTP request
// Ito ang tamang lifetime para sa Database operations
//
// ORDER NG REGISTRATION:
// 1. Repository (nag-a-access ng database)
// 2. Service (gumagamit ng Repository)
// Controller ay automatic na registered ng AddControllers()
// ============================================================
builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddScoped<PokemonService>();

// CORS — para payagan ang frontend (index.html) na mag-call ng API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ============================================================
// PHASE 2: BUILD THE APP
// ============================================================
var app = builder.Build();

// ============================================================
// MIDDLEWARE PIPELINE — Ang bawat HTTP request ay dumadaan dito
// ORDER IS IMPORTANT! Ang unang nakalagay ay unang tatawagan
// ============================================================

// 1. GLOBAL EXCEPTION HANDLER — Dapat UNANG middleware
//    Para mahuli ang lahat ng errors kahit saan sa pipeline
app.UseGlobalExceptionHandler();

// 2. Swagger — available lang sa Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokédex API v1");
        options.RoutePrefix = "swagger";
    });
}

// 3. HTTPS Redirection — redirect HTTP to HTTPS
app.UseHttpsRedirection();

app.UseDefaultFiles();
// 4. Static Files — para ma-serve ang wwwroot/index.html
app.UseStaticFiles();

// 5. CORS — bago ang Controllers
app.UseCors("AllowAll");

// 6. Map Controllers — iru-route ang requests sa tamang Controller
app.MapControllers();

// ============================================================
// SEED DATABASE — I-apply ang migrations at mag-add ng initial data
// Ginagawa ito PAGKATAPOS ng app.Build() pero BAGO mag-run
// ============================================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PokedexDbContext>();

    // Auto-apply pending migrations
    context.Database.Migrate();

    // Mag-seed ng initial data
    SeedData.Initialize(context);
}

// ============================================================
// START THE APPLICATION
// ============================================================
app.Run();