using System.Text;
using BichoApi.Application.Hubs;
using BichoApi.Application.Services.Auth;
using BichoApi.Application.Services.Bet;
using BichoApi.Application.Services.User;
using BichoApi.Domain.Interfaces.Auth;
using BichoApi.Domain.Interfaces.Bet;
using BichoApi.Domain.Interfaces.ILotteryRepository;
using BichoApi.Domain.Interfaces.User;
using BichoApi.Infrastructure.Data.Context;
using BichoApi.Infrastructure.Repositories.Auth;
using BichoApi.Infrastructure.Repositories.Bet;
using BichoApi.Infrastructure.Repositories.Lottery;
using BichoApi.Infrastructure.Repositories.User;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var host = Environment.GetEnvironmentVariable("PGHOST");
var port = Environment.GetEnvironmentVariable("PGPORT");
var db = Environment.GetEnvironmentVariable("PGDATABASE");
var user = Environment.GetEnvironmentVariable("PGUSER");
var password = Environment.GetEnvironmentVariable("PGPASSWORD");
var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")?
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password}";

builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        policy => policy
            .WithOrigins("http://localhost:3000", "https://frontend-bicho-production.up.railway.app")
            // .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("Authorization")
    );
});
var jwtKey = builder.Configuration["JWTKey"];
var keyBytes = Encoding.UTF8.GetBytes(jwtKey!);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSignalR(options => { options.EnableDetailedErrors = true; });


builder.Services.AddDbContext<ApiContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddScoped<IBetRepository, BetRepository>();

builder.Services.AddScoped<ILotteryRepository, LotteryRepository>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BichoApi", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT no campo abaixo.\nExemplo: Bearer eyJhbGciOiJIUzI1NiIsInR...",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});
// builder.Services.AddScoped<BetService>();
builder.Services.AddHostedService<BetService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost3000");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<GameHub>("/jogohub").RequireCors("AllowLocalhost3000");


app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<ApiContext>();
    database.Database.Migrate();
}

app.Run();