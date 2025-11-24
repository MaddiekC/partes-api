using Microsoft.EntityFrameworkCore;
using PartesApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Leer connection string (toma appsettings.Development.json en entorno Development)
var conn = builder.Configuration.GetConnectionString("conn");

// Registrar DbContext usando Pomelo (ServerVersion.AutoDetect detecta versión de MySQL automáticamente)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(conn, ServerVersion.AutoDetect(conn)));

builder.Services.AddCors(options =>
{
    options.AddPolicy("newPolicy",
        app =>
        {
            app.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
