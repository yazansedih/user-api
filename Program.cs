using ExternalApiBackend.Repositories;
using ExternalApiBackend.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Optional: Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// -------------------------------------------
// 1) Register a typed HttpClient for MyRepository
builder.Services.AddHttpClient<IMyRepository, MyRepository>();

// 2) Register other services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IExternalApiService, ExternalApiService>();

// Controllers, endpoints, swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "External API Backend",
        Version = "v1"
    });
});

// Build the app
var app = builder.Build();

// Only use Swagger in Development (optional)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "External API Backend v1"));
}

// Apply CORS policy
app.UseCors("AllowAll");

// For ASP.NET Core 6+ minimal hosting, routing is automatically enabled
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Run the app
app.Run();
