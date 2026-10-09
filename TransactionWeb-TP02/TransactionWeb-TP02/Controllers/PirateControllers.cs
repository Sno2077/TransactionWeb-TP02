using Microsoft.AspNetCore.Mvc;
using TransactionWeb_TP02.Data;
using TransactionWeb_TP02.Models;
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

    //======== /health ========
    [HttpGet("/health")]
    public IActionResult Health()
    {
        return Ok(new { status = "ok" });
    }

    //======== /info ========
    [HttpGet("/info")]
    public IActionResult Info()
    {
        return Ok(new { application = "ServeurPirateTP", version = "0.1.0" });
    }

    //======== GET by Id ========
    [HttpGet("{id:int}")]
    public ActionResult<Pirate> GetById(int id)
    {
        Pirate? pirate = _memory.GetPirates().Find(p => p.Id == id);

        return pirate is null
            ? NotFound(new { error = "Pirate not found" })
            : Ok(pirate);
    }

    //======== GET liste + filtre marine ========
    [HttpGet]
    public IActionResult GetAll([FromQuery] string? marine)
    {
        List<Pirate> pirates = _memory.GetPirates();

        if (marine == null)
            return Ok(pirates);
        if (marine != "true" && marine != "false")
            return BadRequest(new { error = "marine must be 'true' or 'false'" });

        bool marineValue = bool.Parse(marine);
        List<Pirate> filtered = pirates.FindAll(p => p.Marine == marineValue);

        return Ok(filtered);
    }

    //======== POST (CREATE) ========
    [HttpPost]
    public IActionResult Create([FromBody] PirateSimple simple)
    {
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
        catch (JsonException exception)
        {
            return BadRequest(new { error = $"Invalid JSON at {exception.Path}. Expected text for name and type, whole numbers for level and bounty, true or false for marine and available." });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }

    //======== PUT (UPDATE) ========
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] PirateSimple ps)
    {
        List<Pirate> pirates = _memory.GetPirates();
        Pirate? pirate = pirates.Find(p => p.Id == id);

        if (pirate is null)
            return NotFound(new { error = "Pirate not found" });

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
        catch (JsonException exception)
        {
            return BadRequest(new { error = $"Invalid JSON at {exception.Path}. Expected text for name and type, whole numbers for level and bounty, true or false for marine and available." });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }

    //======== DELETE ========
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        List<Pirate> pirates = _memory.GetPirates();
        int index = pirates.FindIndex(p => p.Id == id);

        if (index < 0)
            return NotFound(new { error = "Pirate not found" });

        pirates.RemoveAt(index);
        _memory.SaveToFile();

        return NoContent();
    }
}