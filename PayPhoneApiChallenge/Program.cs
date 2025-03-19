using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PayPhoneApiChallenge.Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PayPhoneApiChallenge.App.Transactions.Interfaces;
using PayPhoneApiChallenge.App.Transactions.Services;
using PayPhoneApiChallenge.App.Wallets.Interfaces;
using PayPhoneApiChallenge.App.Wallets.Services;
using PayPhoneApiChallenge.Milddlewares;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


// DB
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<ITransactionsService, TransactionService>();

builder.Services.AddDbContext<PayPhoneDbContext>(options => options.UseSqlite(connectionString));

// Configurar autenticación JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? String.Empty);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Solo para demo; en producción se debe usar HTTPS
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

// Add Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PayPhone Api Challenge",
        Version = "1.0.0",
        Description = "API para transferencia de saldo entre billeteras."
    });
    // options.EnableAnnotations();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese el token JWT en el formato: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            [] // Returnar Vacio
        }
    });
});

// Crear aplicacion
var app = builder.Build();

// Controlar Errores Centralizados
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // app.UseSwaggerUI(x => {
    //     x.SwaggerEndpoint("/swagger/v1/swagger.json", "PayPhone Api Challenge v1");
    //     x.RoutePrefix = string.Empty;
    // });
    app.UseSwaggerUI();
}
// Redireccionar siempre a https
//app.UseHttpsRedirection();

// Habilitar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapear Controladores
app.MapControllers();

// Iniciar Aplicacion
app.Run();

// Pruebas Integrales
public partial class Program
{
}