//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.JsonPatch;
//using TransactionWeb_TP02.Data;
//using TransactionWeb_TP02.Models;

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
//    public IActionResult Create([FromBody] JsonPatchDocument<PirateSimple> patch)
//    {
//        // Vérifier que le patch n'est pas null
//        if (patch == null)
//            return BadRequest(new { error = "Invalid JSON body. Expected text for name and type, whole numbers for level and bounty, true or false for marine and available." });

//        // Créer un objet vide
//        var simple = new PirateSimple();

//        try
//        {
//            // Appliquer les opérations du patch
//            patch.ApplyTo(simple);
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(new { error = $"Invalid JSON body. {ex.Message}" });
//        }

//        // Validation métier
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

//    //======== PUT (REPLACE) ========
//    [HttpPut("{id:int}")]
//    public IActionResult Update(int id, [FromBody] JsonPatchDocument<PirateSimple> patch)
//    {
//        List<Pirate> pirates = _memory.GetPirates();
//        Pirate? pirate = pirates.Find(p => p.Id == id);

//        if (pirate is null)
//            return NotFound(new { error = "Pirate not found" });

//        // Vérifier que le patch n'est pas null
//        if (patch == null)
//            return BadRequest(new { error = "Invalid JSON body. Expected text for name and type, whole numbers for level and bounty, true or false for marine and available." });

//        // Créer un objet vide et appliquer le patch
//        var ps = new PirateSimple();

//        try
//        {
//            // Appliquer les opérations du patch
//            patch.ApplyTo(ps);
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(new { error = $"Invalid JSON body. {ex.Message}" });
//        }

//        // Validation métier
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

//    //======== PATCH (PARTIAL UPDATE) ========
//    [HttpPatch("{id:int}")]
//    public IActionResult Patch(int id, [FromBody] JsonPatchDocument<PirateSimple> patch)
//    {
//        List<Pirate> pirates = _memory.GetPirates();
//        Pirate? pirate = pirates.Find(p => p.Id == id);

//        if (pirate is null)
//            return NotFound(new { error = "Pirate not found" });

//        if (patch == null)
//            return BadRequest(new { error = "Invalid JSON body. Expected valid patch operations." });

//        // Créer un objet avec les valeurs actuelles du pirate
//        var ps = new PirateSimple
//        {
//            Name = pirate.Name,
//            Type = pirate.Type,
//            Level = pirate.Level,
//            Bounty = pirate.Bounty,
//            Marine = pirate.Marine,
//            Available = pirate.Available
//        };

//        try
//        {
//            // Appliquer les opérations du patch
//            patch.ApplyTo(ps);
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(new { error = $"Invalid JSON body. {ex.Message}" });
//        }

//        // Validation métier
//        try
//        {
//            pirate.Name = ps.Name!;
//            pirate.Type = ps.Type!;
//            pirate.Level = ps.Level;
//            pirate.Bounty = ps.Bounty;
//            pirate.Marine = ps.Marine;
//            pirate.Available = ps.Available;

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