using App.Database;
using App.Endpoints;
using App.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddNpgsql<ApplicationDbContext>(connString);

builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();


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

app.MapGameEndpoints();
app.MapUserEndpoints();
app.Run();
