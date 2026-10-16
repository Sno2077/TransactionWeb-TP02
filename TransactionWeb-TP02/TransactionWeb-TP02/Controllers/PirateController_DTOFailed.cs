//using Microsoft.AspNetCore.Mvc;
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
//    public IActionResult Create([FromBody] PirateDTO dto)
//    {
//        if (dto == null)
//            return BadRequest(new { error = "Invalid JSON body. Expected text for name and type, whole numbers for level and bounty, true or false for marine and available." });

//        try
//        {
//            // Conversion des champs string vers les types appropriés
//            int level = int.Parse(dto.Level ?? throw new FormatException("Level is required"));
//            int bounty = int.Parse(dto.Bounty ?? throw new FormatException("Bounty is required"));
//            bool marine = bool.Parse(dto.Marine ?? throw new FormatException("Marine is required"));
//            bool available = bool.Parse(dto.Available ?? throw new FormatException("Available is required"));

//            // Validation métier (les setters du Pirate valideront aussi)
//            List<Pirate> pirates = _memory.GetPirates();

//            Pirate newPirate = new Pirate
//            {
//                Id = pirates.Count == 0 ? 1 : pirates.Max(p => p.Id) + 1,
//                Name = dto.Name ?? throw new ArgumentException("Name is required"),
//                Type = dto.Type ?? throw new ArgumentException("Type is required"),
//                Level = level,
//                Bounty = bounty,
//                Marine = marine,
//                Available = available
//            };

//            pirates.Add(newPirate);
//            _memory.SaveToFile();

//            return CreatedAtAction(nameof(GetById), new { id = newPirate.Id }, newPirate);
//        }
//        catch (FormatException ex)
//        {
//            return BadRequest(new { error = $"Invalid JSON body. {ex.Message}" });
//        }
//        catch (ArgumentException ex)
//        {
//            return BadRequest(new { error = ex.Message });
//        }
//    }

//    //======== PUT (UPDATE) ========
//    [HttpPut("{id:int}")]
//    public IActionResult Update(int id, [FromBody] PirateDTO dto)
//    {
//        List<Pirate> pirates = _memory.GetPirates();
//        Pirate? pirate = pirates.Find(p => p.Id == id);

//        if (pirate is null)
//            return NotFound(new { error = "Pirate not found" });

//        if (dto == null)
//            return BadRequest(new { error = "Invalid JSON body. Expected text for name and type, whole numbers for level and bounty, true or false for marine and available." });

//        try
//        {
//            // Conversion des champs string vers les types appropriés
//            int level = int.Parse(dto.Level ?? throw new FormatException("Level is required"));
//            int bounty = int.Parse(dto.Bounty ?? throw new FormatException("Bounty is required"));
//            bool marine = bool.Parse(dto.Marine ?? throw new FormatException("Marine is required"));
//            bool available = bool.Parse(dto.Available ?? throw new FormatException("Available is required"));

//            // Validation métier
//            pirate.Name = dto.Name ?? throw new ArgumentException("Name is required");
//            pirate.Type = dto.Type ?? throw new ArgumentException("Type is required");
//            pirate.Level = level;
//            pirate.Bounty = bounty;
//            pirate.Marine = marine;
//            pirate.Available = available;

//            _memory.SaveToFile();
//            return Ok(pirate);
//        }
//        catch (FormatException ex)
//        {
//            return BadRequest(new { error = $"Invalid JSON body. {ex.Message}" });
//        }
//        catch (ArgumentException ex)
//        {
//            return BadRequest(new { error = ex.Message });
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
