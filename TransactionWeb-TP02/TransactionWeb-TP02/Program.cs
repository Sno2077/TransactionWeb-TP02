using TransactionWeb_TP02.Data;
using TransactionWeb_TP02.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<PirateMemory>();
builder.Services.AddSingleton<ExpeditionMemory>();
builder.Services.AddSingleton<AssignmentMemory>();
builder.Services.AddSingleton<ExpeditionWorkflowService>();

WebApplication app = builder.Build();

app.MapControllers();

app.MapFallback(() => Results.NotFound(new { error = "Unknown route" }));

app.Run();