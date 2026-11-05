using Microsoft.AspNetCore.Mvc;

namespace TransactionWeb_TP03.Controllers;

/// <summary>Expose les routes publiques de santé et d'information du serveur.</summary>
[ApiController]
public class SystemController : ControllerBase
{
    /// <summary>Indique que le serveur répond.</summary>
    [HttpGet("/health")]
    public IActionResult Health()
    {
        return Ok(new { status = "ok" });
    }

    /// <summary>Retourne le nom et la version de l'application.</summary>
    [HttpGet("/info")]
    public IActionResult Info()
    {
        return Ok(new { application = "ServeurPirateTP", version = "0.3.0" });
    }
}