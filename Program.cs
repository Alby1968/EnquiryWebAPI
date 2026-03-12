using ApiNetCoreAngular.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ===== Add services =====
builder.Services.AddControllers();

// Database: leggere connection string da Environment su Render
builder.Services.AddDbContext<EnquiryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS: consentire solo il tuo frontend Netlify
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNetlify", policy =>
    {
        policy.WithOrigins("https://enquiryapiangular.netlify.app")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("Enquiry", new OpenApiInfo
    {
        Title = "Enquiry API",
        Version = "V1"
    });
});

var app = builder.Build();

// ===== Configure middleware =====

// Abilita Swagger in produzione e sviluppo
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/Enquiry/swagger.json", "Enquiry API");
    options.RoutePrefix = "swagger"; // accesso via /swagger
});

app.UseHttpsRedirection();

app.UseCors("AllowNetlify");

app.UseAuthorization();

app.MapControllers();

// ===== Porta dinamica per Render =====
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");