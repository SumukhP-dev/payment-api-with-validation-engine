using Microsoft.OpenApi.Models;           // For OpenAPI/Swagger
using PaymentApi.Services;                // Your custom services namespace

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Add services to the container
builder.Services.AddControllers();

// ✅ Add Swagger / OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Payment Processing API",
        Version = "v1",
        Description = "An API for handling payment processing with Swagger documentation."
    });
    
    // Include XML comments for Swagger examples
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Register your Payment Service
builder.Services.AddSingleton<IPaymentService, PaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() ||
    app.Environment.IsStaging() ||
    app.Environment.IsProduction())
{
    // Enable Swagger middleware
    app.UseSwagger();

    // ✅ Enable Swagger UI at root URL (/)
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Processing API v1");
        options.RoutePrefix = string.Empty; // Swagger UI served at http://localhost:5000/
    });
}

app.UseRouting();
app.UseCors("AllowAll");
// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();