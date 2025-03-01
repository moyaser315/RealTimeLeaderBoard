

using App.Database;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddNpgsql<ApplicationDbContext>(connString); //Scoped Lifetime
var app = builder.Build();

// GET /games


app.Run();
// Add transient --> lightweight service used once
// Add Scoped --> will be used during scope of a single request
// Add Singleton --> will be used as a single instance for all requests