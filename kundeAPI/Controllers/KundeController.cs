using kundeAPI.Models;
using kundeAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace kundeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KundeController : ControllerBase
{
    private readonly ILogger<KundeController> _logger;

    private readonly KundeService _kundeService;

    public KundeController(ILogger<KundeController> logger, KundeService kundeService)
    {
        _logger = logger;
        _kundeService = kundeService;
    }

    [HttpGet]
    public async Task<List<Kunde>> Get() =>
        await _kundeService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Kunde>> Get(string id)
    {
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
        await _kundeService.CreateAsync(newKunde);

        return CreatedAtAction(nameof(Get), new { id = newKunde.Id }, newKunde);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Kunde updatedKunde)
    {
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
        var Kunde = await _kundeService.GetAsync(id);

        if (Kunde is null)
        {
            return NotFound();
        }

        await _kundeService.RemoveAsync(id);

        return NoContent();
    }
}
