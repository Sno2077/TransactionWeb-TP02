namespace TransactionWeb_TP03;

/// <summary>Assemble le serveur et le démarre.</summary>
internal static class Program
{
    /// <summary>Démarre le serveur des expéditions.</summary>
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        WebApplication app = builder.Build();
        app.MapControllers();
        app.MapFallback(() => Results.Problem(detail: "Unknown route.", statusCode: 404));
        app.Run();
    }
}