using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using PeliculasMinimalAPI;
using PeliculasMinimalAPI.Endpoints;
using PeliculasMinimalAPI.Entidades;
using PeliculasMinimalAPI.Migrations;
using PeliculasMinimalAPI.Repositorio;
using PeliculasMinimalAPI.Servicios;




var builder = WebApplication.CreateBuilder(args);

var origenesPermitidos = builder.Configuration.GetValue<String>("origenesPermitidos")!;
//Inicio de area de servicios
builder.Services.AddDbContext<ApplicationDBContext>(opciones =>
opciones.UseSqlServer("name=DefaultConnection"));

   
builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(configuration =>
    {
        configuration.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();

    });
    opciones.AddPolicy("libre" , configuration =>
    {
        configuration.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

    });
});

builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IRepositorioGeneros, RepositorioGenero>();
builder.Services.AddScoped<IRepositorioActores, RepositorioActores>();
builder.Services.AddScoped<IRepositorioPeliculas, RepositorioPeliculas>();
builder.Services.AddScoped<IRepositorioComentarios, RepositorioComentarios>();

builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Fin de area de servicios
var app = builder.Build();
//Inicio de area de los middleware

/*if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseCors();
 
app.UseOutputCache();
app.MapGet("/", () => "Hello World!");

app.MapGroup("/generos").MapGeneros();
app.MapGroup("/actores").MapActores();
app.MapGroup("/peliculas").MapPeliculas();
app.MapGroup("/pelicula/{peliculaId:int}/comentarios").MapComentarios();



app.Run();


