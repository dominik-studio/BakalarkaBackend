using Microsoft.AspNetCore.Mvc;
using CRMBackend.Application.ObjednavkaAggregate.Commands.CenovePonuky.CreateCenovaPonuka;
using CRMBackend.Application.ObjednavkaAggregate.Commands.CenovePonuky.PatchCenovaPonuka;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace CRMBackend.Web.Controllers;

// [Authorize]
[ApiController]
[Route("api/objednavky/{objednavkaId}/cenove-ponuky")]
public class ObjednavkyCenovePonukyController : ControllerBase
{
    private readonly ISender _sender;

    public ObjednavkyCenovePonukyController(ISender sender) => _sender = sender;

    [HttpPost]
    public async Task<IActionResult> Create(int objednavkaId, CreateCenovaPonukaCommand command)
    {
        if (command.ObjednavkaId != objednavkaId) return BadRequest("Route objednavkaId does not match command objednavkaId");
        var entityId = await _sender.Send(command);
        return CreatedAtAction("", new { ponukaId = entityId });
    }

    [HttpPatch("{ponukaId}")]
    public async Task<IActionResult> Patch(int objednavkaId, int ponukaId, PatchCenovaPonukaCommand command)
    {
        if (command.ObjednavkaId != objednavkaId || command.CenovaPonukaId != ponukaId) 
            return BadRequest("Route IDs do not match command IDs");
        
        await _sender.Send(command);
        return NoContent();
    }
} 
