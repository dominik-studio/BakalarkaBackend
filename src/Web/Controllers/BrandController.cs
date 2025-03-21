using Microsoft.AspNetCore.Mvc;
using Plainquire.Filter;
using Plainquire.Page;
using Plainquire.Sort;
using PromobayBackend.Application.BrandAggregate.Commands.CreateBrand;
using PromobayBackend.Application.BrandAggregate.Commands.DeleteBrand;
using PromobayBackend.Application.BrandAggregate.Commands.UpdateBrand;
using PromobayBackend.Application.BrandAggregate.Queries.GetBrands;
using PromobayBackend.Domain.AggregateRoots.BrandAggregate;

namespace PromobayBackend.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandController : ControllerBase
{
    private readonly ISender _sender;

    public BrandController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<BrandDto>>> Get(
        [FromQuery] EntityFilter<Brand> filter,
        [FromQuery] EntitySort<Brand> sort,
        [FromQuery] EntityPage<Brand> page)
    {
        var brands = await _sender.Send(new GetBrandsQuery { Filter = filter, Sort = sort, Page = page });
        return Ok(brands);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateBrandCommand command)
    {
        var brandId = await _sender.Send(command);
        return Ok(brandId);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateBrandCommand command)
    {
        if (id != command.Id)
            return BadRequest("Route ID does not match command ID");

        await _sender.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _sender.Send(new DeleteBrandCommand(id));
        return NoContent();
    }
} 
