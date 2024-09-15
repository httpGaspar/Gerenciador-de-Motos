using Microsoft.EntityFrameworkCore;
using MotoApi.Data; // MotoContext
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do banco de dados (PostgreSQL)
builder.Services.AddDbContext<MotoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MotoContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.

// Swagger configurado para qualquer ambiente
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Moto API V1");
    c.RoutePrefix = string.Empty; // Define Swagger como página inicial
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // Mapeia os controladores para rotas

app.Run();
