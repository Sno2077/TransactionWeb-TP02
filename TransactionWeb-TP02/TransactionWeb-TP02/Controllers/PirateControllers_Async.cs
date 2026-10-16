//using Microsoft.AspNetCore.Mvc;
//using TransactionWeb_TP02.Data;
//using TransactionWeb_TP02.Models;
//using System.Text.Json;

//namespace TransactionWeb_TP02.Controllers;

//[ApiController]
//[Route("api/pirates")]
//public class PiratesController : ControllerBase
//{
//    private readonly PirateMemory _memory;

//    public PiratesController(PirateMemory memory)
//    {
//        _memory = memory;
//    }

//    [HttpGet("/health")]
//    public IActionResult Health() => Ok(new { status = "ok" });

//    [HttpGet("/info")]
//    public IActionResult Info() => Ok(new { application = "ServeurPirateTP", version = "0.1.0" });

//    //===== GET (READ) =====
//    [HttpGet("{id}")]
//    public ActionResult<Pirate> GetById(string id)
//    {
//        if (!int.TryParse(id, out int parsedId))
//            return BadRequest(new { error = "Invalid ID. Expected a whole number." });

//        Pirate? pirate = _memory.GetPirates().Find(p => p.Id == parsedId);
//        return pirate is null ? NotFound(new { error = "Pirate not found" }) : Ok(pirate);
//    }

//    [HttpGet]
//    public IActionResult GetAll([FromQuery] string? marine)
//    {
//        List<Pirate> pirates = _memory.GetPirates();

//        if (marine == null)
//            return Ok(pirates);
//        if (marine != "true" && marine != "false")
//            return BadRequest(new { error = "marine must be 'true' or 'false'" });

//        bool marineValue = bool.Parse(marine);
//        return Ok(pirates.FindAll(p => p.Marine == marineValue));
//    }

//    //======== POST (CREATE) ========
//    [HttpPost]
//    public async Task<IActionResult> Create()
//    {
//        // Lire le body directement (pas de [FromBody])
//        using var reader = new StreamReader(Request.Body);
//        string json = await reader.ReadToEndAsync();

//        if (string.IsNullOrWhiteSpace(json))
//            return BadRequest(new { error = "Request body is empty." });

//        PirateSimple? simple = null;

//        // Étape 1 : Parser le JSON
//        try
//        {
//            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//            simple = JsonSerializer.Deserialize<PirateSimple>(json, options);

//            if (simple == null)
//                return BadRequest(new { error = "Invalid JSON body. Expected text for name and type, whole numbers for level and bounty, true or false for marine and available." });
//        }
//        catch (JsonException exception)
//        {
//            return BadRequest(new { error = $"Invalid JSON at {exception.Path}. Expected text for name and type, whole numbers for level and bounty, true or false for marine and available." });
//        }

//        // Étape 2 : Validation métier
//        try
//        {
//            List<Pirate> pirates = _memory.GetPirates();

//            Pirate newPirate = new Pirate
//            {
//                Id = pirates.Count == 0 ? 1 : pirates.Max(p => p.Id) + 1,
//                Name = simple.Name!,
//                Type = simple.Type!,
//                Level = simple.Level,
//                Bounty = simple.Bounty,
//                Marine = simple.Marine,
//                Available = simple.Available
//            };

//            pirates.Add(newPirate);
//            _memory.SaveToFile();

//            return CreatedAtAction(nameof(GetById), new { id = newPirate.Id }, newPirate);
//        }
//        catch (ArgumentException exception)
//        {
//            return BadRequest(new { error = exception.Message });
//        }
//    }

//    //======== PUT (UPDATE) ========
//    [HttpPut("{id:int}")]
//    public async Task<IActionResult> Update(int id)
//    {
//        List<Pirate> pirates = _memory.GetPirates();
//        Pirate? pirate = pirates.Find(p => p.Id == id);

//        if (pirate is null)
//            return NotFound(new { error = "Pirate not found" });

//        // Lire le body directement (pas de [FromBody])
//        using var reader = new StreamReader(Request.Body);
//        string json = await reader.ReadToEndAsync();

//        if (string.IsNullOrWhiteSpace(json))
//            return BadRequest(new { error = "Request body is empty." });

//        PirateSimple? ps = null;

//        // Étape 1 : Parser le JSON
//        try
//        {
//            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//            ps = JsonSerializer.Deserialize<PirateSimple>(json, options);

//            if (ps == null)
//                return BadRequest(new { error = "Invalid JSON body. Expected text for name and type, whole numbers for level and bounty, true or false for marine and available." });
//        }
//        catch (JsonException exception)               
//        {
//            return BadRequest(new { error = $"Invalid JSON at {exception.Path}. Expected text for name and type, whole numbers for level and bounty, true or false for marine and available." });
//        }

//        // Étape 2 : Validation métier
//        try
//        {
//            Pirate validated = new Pirate
//            {
//                Id = id,
//                Name = ps.Name!,
//                Type = ps.Type!,
//                Level = ps.Level,
//                Bounty = ps.Bounty,
//                Marine = ps.Marine,
//                Available = ps.Available
//            };

//            pirate.Name = validated.Name;
//            pirate.Type = validated.Type;
//            pirate.Level = validated.Level;
//            pirate.Bounty = validated.Bounty;
//            pirate.Marine = validated.Marine;
//            pirate.Available = validated.Available;

//            _memory.SaveToFile();
//            return Ok(pirate);
//        }
//        catch (ArgumentException exception)
//        {
//            return BadRequest(new { error = exception.Message });
//        }
//    }

//    //======== DELETE ========
//    [HttpDelete("{id:int}")]
//    public IActionResult Delete(int id)
//    {
//        List<Pirate> pirates = _memory.GetPirates();
//        int index = pirates.FindIndex(p => p.Id == id);

//        if (index < 0)
//            return NotFound(new { error = "Pirate not found" });

//        pirates.RemoveAt(index);
//        _memory.SaveToFile();

//        return NoContent();
//    }
//}