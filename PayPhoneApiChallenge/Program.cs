using System;
using PayPhoneApiChallenge.Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PayPhoneApiChallenge.App.Transactions.Interfaces;
using PayPhoneApiChallenge.App.Transactions.Services;
using PayPhoneApiChallenge.App.Wallets.Interfaces;
using PayPhoneApiChallenge.App.Wallets.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


// Inicializar DB
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<ITransactionsService, TransactionService>();
builder.Services.AddDbContext<PayPhoneDbContext>(options => options.UseSqlite(connectionString));

// Add Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PayPhone Api Challenge", Version = "v1" });
});

var app = builder.Build();

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

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();
app.Run();