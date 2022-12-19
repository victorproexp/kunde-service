using kundeAPI.Models;
using kundeAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace kundeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KundeController : ControllerBase
{
    private readonly ILogger<KundeController> _logger;

    private readonly IKundeService _kundeService;

    public KundeController(ILogger<KundeController> logger, IKundeService kundeService)
    {
        _logger = logger;
        _logger.LogDebug(1, "NLog injected into KundeController");
        _kundeService = kundeService;
    }

    [HttpGet]
    public async Task<List<Kunde>> Get() =>
        await _kundeService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Kunde>> Get(string id)
    {
        _logger.LogInformation("Get called at {DT}",
            DateTime.UtcNow.ToLongTimeString());
                
        var Kunde = await _kundeService.GetAsync(id);

        if (Kunde is null)
        {
            return NotFound();
        }

        return Kunde;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Kunde newKunde)
    {
        _logger.LogInformation("Post called at {DT}",
            DateTime.UtcNow.ToLongTimeString());

        await _kundeService.CreateAsync(newKunde);

        return CreatedAtAction(nameof(Get), new { id = newKunde.Id }, newKunde);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Kunde updatedKunde)
    {
        _logger.LogInformation("Update called at {DT}",
            DateTime.UtcNow.ToLongTimeString());

        var Kunde = await _kundeService.GetAsync(id);

        if (Kunde is null)
        {
            return NotFound();
        }

        updatedKunde.Id = Kunde.Id;

        await _kundeService.UpdateAsync(id, updatedKunde);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        _logger.LogInformation("Delete called at {DT}",
            DateTime.UtcNow.ToLongTimeString());

        var Kunde = await _kundeService.GetAsync(id);

        if (Kunde is null)
        {
            return NotFound();
        }

        await _kundeService.RemoveAsync(id);

        return NoContent();
    }

    [HttpGet("version")]
    public IEnumerable<string> GetVersion()
    {
        var properties = new List<string>();
        var assembly = typeof(Program).Assembly;
        foreach (var attribute in assembly.GetCustomAttributesData())
        {
            properties.Add($"{attribute.AttributeType.Name} - {attribute.ToString()}");
        }
        return properties;
    }
}
