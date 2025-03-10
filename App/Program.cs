using System.Text;
using App.Database;
using App.Endpoints;
using App.Services;
using App.Validators;
using FluentValidation;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connStringDb = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddNpgsql<ApplicationDbContext>(connStringDb);

var connStringCache = builder.Configuration.GetConnectionString("redis");
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(connStringCache));
builder.Services.AddScoped<IRedisCacheService,RedisCacheService>();


builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(
    jwtSettings["SignKey"]?? throw new ArgumentNullException("JWT secret key is missing in configuration.")
    );
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero 
        };
    });
builder.Services.AddScoped<IAuthenicationService,AuthenicationService>();

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "LeaderboardAPI";
    config.Title = "LeaderboardAPI v1";
    config.Version = "v1";
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "LeaderboardAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}
app.UseAuthentication();
app.UseAuthorization();
app.MapGameEndpoints();
app.MapUserEndpoints();
app.Run();
