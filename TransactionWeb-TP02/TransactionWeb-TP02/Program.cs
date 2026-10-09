using TransactionWeb_TP02.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<PirateMemory>();

WebApplication app = builder.Build();

app.MapControllers();

app.MapFallback(() => Results.NotFound(new { error = "Unknown route" }));

app.Run("http://localhost:5050");