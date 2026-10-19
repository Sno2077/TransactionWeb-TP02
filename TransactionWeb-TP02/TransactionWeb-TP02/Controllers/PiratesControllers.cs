using Microsoft.AspNetCore.Mvc;
using TransactionWeb_TP02.Data;
using TransactionWeb_TP02.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace TransactionWeb_TP02.Controllers;

[ApiController]
[Route("api/pirates")]
public class PiratesController : ControllerBase
{
    private readonly PirateMemory _memory;

    public PiratesController(PirateMemory memory)
    {
        _memory = memory;
    }

    [HttpGet("/health")]
    public IActionResult Health() => Ok(new { status = "ok" });

    [HttpGet("/info")]
    public IActionResult Info() => Ok(new { application = "ServeurPirateTP", version = "0.1.0" });

    //===== GET (READ) =====
    [HttpGet("{id}")]
    public ActionResult<Pirate> GetById(string id)
    {
        if (!int.TryParse(id, out int parsedId))
            return Problem(detail: "Invalid ID. Expected a whole number.", statusCode: 400);

        Pirate? pirate = _memory.GetPirates().Find(p => p.Id == parsedId);
        return pirate is null ? Problem(detail: "Pirate not found", statusCode: 404) : Ok(pirate);
    }

    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] bool? marine,
        [FromQuery] bool? available,
        [FromQuery, Range(1, 100)] int limit = 10)
    {
        IEnumerable<Pirate> pirates = _memory.GetPirates();

        if (marine.HasValue)
            pirates = pirates.Where(p => p.Marine == marine.Value);

        if (available.HasValue)
            pirates = pirates.Where(p => p.Available == available.Value);

        return Ok(pirates.OrderBy(p => p.Name).ThenBy(p => p.Id).Take(limit).ToList());
    }

    //======== POST (CREATE) ========
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        // Lire le body directement (pas de [FromBody])
        using var reader = new StreamReader(Request.Body);
        string json = await reader.ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(json))
            return Problem(detail: "Request body is empty.", statusCode: 400);

        PirateSimple? simple = null;

        // Étape 1 : Parser le JSON
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            simple = JsonSerializer.Deserialize<PirateSimple>(json, options);

            if (simple == null)
                return Problem(detail: "Invalid JSON body. Expected text for name and type, whole numbers for level and bounty, true or false for marine and available.", statusCode: 400);
        }
        catch (JsonException exception)
        {
            return Problem(detail: $"Invalid JSON at {exception.Path}. Expected text for name and type, whole numbers for level and bounty, true or false for marine and available.", statusCode: 400);
        }

        // Étape 2 : Validation métier
        try
        {
            List<Pirate> pirates = _memory.GetPirates();

            Pirate newPirate = new Pirate
            {
                Id = pirates.Count == 0 ? 1 : pirates.Max(p => p.Id) + 1,
                Name = simple.Name!,
                Type = simple.Type!,
                Level = simple.Level,
                Bounty = simple.Bounty,
                Marine = simple.Marine,
                Available = simple.Available
            };

            pirates.Add(newPirate);
            _memory.SaveToFile();

            return CreatedAtAction(nameof(GetById), new { id = newPirate.Id }, newPirate);
        }
        catch (ArgumentException exception)
        {
            return Problem(detail: exception.Message, statusCode: 400);
        }
    }

    //======== PUT (UPDATE) ========
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id)
    {
        List<Pirate> pirates = _memory.GetPirates();
        Pirate? pirate = pirates.Find(p => p.Id == id);

        if (pirate is null)
            return Problem(detail: "Pirate not found", statusCode: 404);

        // Lire le body directement (pas de [FromBody])
        using var reader = new StreamReader(Request.Body);
        string json = await reader.ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(json))
            return Problem(detail: "Request body is empty.", statusCode: 400);

        PirateSimple? ps = null;

        // Étape 1 : Parser le JSON
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            ps = JsonSerializer.Deserialize<PirateSimple>(json, options);

            if (ps == null)
                return Problem(detail: "Invalid JSON body. Expected text for name and type, whole numbers for level and bounty, true or false for marine and available.", statusCode: 400);
        }
        catch (JsonException exception)
        {
            return Problem(detail: $"Invalid JSON at {exception.Path}. Expected text for name and type, whole numbers for level and bounty, true or false for marine and available.", statusCode: 400);
        }

        // Étape 2 : Validation métier
        try
        {
            Pirate validated = new Pirate
            {
                Id = id,
                Name = ps.Name!,
                Type = ps.Type!,
                Level = ps.Level,
                Bounty = ps.Bounty,
                Marine = ps.Marine,
                Available = ps.Available
            };

            pirate.Name = validated.Name;
            pirate.Type = validated.Type;
            pirate.Level = validated.Level;
            pirate.Bounty = validated.Bounty;
            pirate.Marine = validated.Marine;
            pirate.Available = validated.Available;

            _memory.SaveToFile();
            return Ok(pirate);
        }
        catch (ArgumentException exception)
        {
            return Problem(detail: exception.Message, statusCode: 400);
        }
    }

    //======== DELETE ========
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        List<Pirate> pirates = _memory.GetPirates();
        int index = pirates.FindIndex(p => p.Id == id);

        if (index < 0)
            return Problem(detail: "Pirate not found", statusCode: 404);

        pirates.RemoveAt(index);
        _memory.SaveToFile();

        return NoContent();
    }
}