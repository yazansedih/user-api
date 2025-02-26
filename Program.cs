using ExternalApiBackend.Repositories;
using ExternalApiBackend.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Enable CORS (Cross-Origin Requests)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// ✅ Register HttpClient for External API Calls
builder.Services.AddHttpClient<IExternalApiRepository, ExternalApiRepository>();

// ✅ Register JSON File Repository & Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// ✅ Register Services
builder.Services.AddScoped<IExternalApiService, ExternalApiService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ✅ Add Swagger Configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "External API Backend",
        Version = "v1",
        Description = "A .NET Core API interacting with external APIs and JSON file storage",
        Contact = new OpenApiContact
        {
            Name = "Developer Support",
            Email = "support@example.com",
            Url = new Uri("https://example.com/contact")
        }
    });
});

// ✅ Build the application
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "External API Backend v1"));
}

// ✅ Enable CORS
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();
