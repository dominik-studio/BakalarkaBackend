using Microsoft.AspNetCore.Mvc;
using PromobayBackend.Application.TodoLists.Commands.CreateTodoList;
using PromobayBackend.Application.TodoLists.Commands.DeleteTodoList;
using PromobayBackend.Application.TodoLists.Commands.UpdateTodoList;
using PromobayBackend.Application.TodoLists.Commands.UpdateTodoListMaxItems;
using PromobayBackend.Application.TodoLists.Commands.AddTodoItem;
using PromobayBackend.Application.TodoLists.Commands.RemoveTodoItem;
using PromobayBackend.Application.TodoLists.Commands.UpdateTodoItem;
using PromobayBackend.Application.TodoLists.Queries.GetTodos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Plainquire.Filter;
using Plainquire.Page;
using Plainquire.Sort;
using PromobayBackend.Domain.Entities;

namespace PromobayBackend.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoListController : ControllerBase
{
    private readonly ISender _sender;

    public TodoListController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] EntityFilter<TodoList> filter,
        [FromQuery] EntitySort<TodoList> sort,
        [FromQuery] EntityPage<TodoList> page)
    {
        var result = await _sender.Send(new GetTodosQuery
        {
            Filter = filter,
            Sort = sort,
            Page = page
        });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTodoListCommand command)
    {
        var result = await _sender.Send(command);
        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTodoListCommand command)
    {
        if (command.Id != id) return BadRequest("Route ID does not match command ID");
        await _sender.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/MaxItems")]
    public async Task<IActionResult> UpdateMaxItems(int id, UpdateTodoListMaxItemsCommand command)
    {
        if (command.Id != id) return BadRequest("Route ID does not match command ID");
        await _sender.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _sender.Send(new DeleteTodoListCommand(id));
        return NoContent();
    }

    [HttpPost("{listid}/Items")]
    public async Task<IActionResult> AddItem(int listid, AddTodoItemCommand command)
    {
        if (command.ListId != listid) return BadRequest("Route List ID does not match command List ID");
        var result = await _sender.Send(command);
        return Created();
    }

    [HttpPut("{listid}/Items/{itemid}")]
    public async Task<IActionResult> UpdateItem(int listid, int itemid, UpdateTodoItemCommand command)
    {
        if (command.ListId != listid || command.ItemId != itemid) return BadRequest("Route IDs do not match command IDs");
        await _sender.Send(command);
        return NoContent();
    }

    [HttpDelete("{listid}/Items/{itemid}")]
    public async Task<IActionResult> RemoveItem(int listid, int itemid)
    {
        await _sender.Send(new RemoveTodoItemCommand(listid, itemid));
        return NoContent();
    }
}
