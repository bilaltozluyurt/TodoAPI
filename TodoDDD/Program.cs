using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoDDD.Application.Interfaces;
using TodoDDD.Application.Services;
using TodoDDD.Infrastructure.Data;
using TodoDDD.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// appsettings.json'dan DatabaseProvider deðerini oku
var dbProvider = builder.Configuration["DatabaseProvider"]?.ToLower();

if (!args.Contains("ef"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        if (dbProvider == "postgres")
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
        }
        else if (dbProvider == "sqlserver")
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
        }
        else
        {
            throw new Exception("Geçersiz database provider ayarý: " + dbProvider);
        }
    });
}

// Dependency Injection (DI)
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddControllers();

// Swagger/OpenAPI için
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
