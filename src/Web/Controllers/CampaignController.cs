using Microsoft.AspNetCore.Mvc;
using PromobayBackend.Application.CampaignAggregate.Commands.CreateCampaign;
using PromobayBackend.Application.CampaignAggregate.Commands.DeleteCampaign;
using PromobayBackend.Application.CampaignAggregate.Commands.UpdateCampaign;
using PromobayBackend.Application.CampaignAggregate.Commands.UpdateCampaignStatus;
using PromobayBackend.Application.CampaignAggregate.Commands.UpdateCampaignDates;
using PromobayBackend.Application.CampaignAggregate.Queries.GetCampaigns;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Plainquire.Filter;
using Plainquire.Page;
using Plainquire.Sort;
using PromobayBackend.Domain.AggregateRoots.CampaignAggregate;

namespace PromobayBackend.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampaignController : ControllerBase
{
    private readonly ISender _sender;

    public CampaignController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] EntityFilter<Campaign> filter,
        [FromQuery] EntitySort<Campaign> sort,
        [FromQuery] EntityPage<Campaign> page)
    {
        var result = await _sender.Send(new GetCampaignsQuery
        {
            Filter = filter,
            Sort = sort,
            Page = page
        });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCampaignCommand command)
    {
        var result = await _sender.Send(command);
        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCampaignCommand command)
    {
        if (command.Id != id) return BadRequest("Route ID does not match command ID");
        await _sender.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/Status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateCampaignStatusCommand command)
    {
        if (command.Id != id) return BadRequest("Route ID does not match command ID");
        await _sender.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/Dates")]
    public async Task<IActionResult> UpdateDates(int id, UpdateCampaignDatesCommand command)
    {
        if (command.Id != id) return BadRequest("Route ID does not match command ID");
        await _sender.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _sender.Send(new DeleteCampaignCommand(id));
        return NoContent();
    }
} 
