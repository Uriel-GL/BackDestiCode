using BackDestiCode.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(StartupBase));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configuracion de la cadena de conexion a Base de datos con SQL
builder.Services.AddDbContext<ApiDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StrConnection"));
});

// Configuracion de la inyeccion de dependencias de las Interfaces y Repositorios de la app  


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//Configuracion del uso de cors de la Api
app.UseCors(cors => cors.AllowAnyHeader().AllowAnyMethod().WithOrigins("*"));

app.MapControllers();

app.Run();
