using Microsoft.EntityFrameworkCore;
using TaskApp.Application;
using TaskApp.Application.EventHandlers;
using TaskApp.Domain.Shared;
using TaskApp.Infrastructure;
using TaskApp.Infrastructure.Persistence;
using TaskApp.WepApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultDB");
var urlUI = configuration["URLs:BaseUI"] ?? throw new Exception("Url base not available");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.MaxBatchSize(100);
    }));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:8080", "http://localhost:9191", urlUI)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Application + Infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

// Controllers & AutoMapper
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Global exception handler 
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskApp API V1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseCors("AllowReactApp");
app.UseExceptionHandler("/Error");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
