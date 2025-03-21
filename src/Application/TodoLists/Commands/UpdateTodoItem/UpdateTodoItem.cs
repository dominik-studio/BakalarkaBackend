using System.Diagnostics;
using MediatR;
using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.Entities;
using PromobayBackend.Domain.Enums;

namespace PromobayBackend.Application.TodoLists.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand : IRequest
{
    public int ListId { get; init; }
    public int ItemId { get; init; }
    public string? Title { get; init; }
    public string? Note { get; init; }
    public PriorityLevel? Priority { get; init; }
    public DateTime? Reminder { get; init; }
    public bool? Done { get; init; }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand>
{
    private readonly IWriteRepository<TodoList> _repository;

    public UpdateTodoItemCommandHandler(IWriteRepository<TodoList> repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.ListId);
        Guard.Against.Null(request.ItemId);
        Guard.Against.Null(request.Title);
        Guard.Against.Null(request.Done);
        
        var todoList = await _repository.GetByIdAsync(request.ListId.Value, cancellationToken);
        Guard.Against.NotFound(request.ListId.Value, todoList);

        var todoItem = todoList.Items.FirstOrDefault(i => i.Id == request.ItemId.Value);
        Guard.Against.NotFound(request.ItemId.Value, todoItem);

        todoItem.Title = request.Title;
        todoItem.Note = request.Note;
        todoItem.Priority = request.Priority;
        todoItem.Reminder = request.Reminder;
        todoItem.Done = (bool)request.Done;

        await _repository.SaveAsync(cancellationToken);
    }
} 
