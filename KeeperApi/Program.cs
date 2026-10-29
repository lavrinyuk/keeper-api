using KeeperApi.Data;
using KeeperApi.Services;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = "Host=localhost;Port=5432;Database=keeper_db;Username=postgres;Password=postgres";
}

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseNpgsql(connectionString, npgsqlOptions => {
        npgsqlOptions.EnableRetryOnFailure();
    });
});

builder.Services.AddScoped<KeeperService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
