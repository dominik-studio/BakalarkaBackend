using CRMBackend.Application.Common.Interfaces.Data;
using CRMBackend.Domain.Entities;
using CRMBackend.Domain.ValueObjects;

namespace CRMBackend.Application.TodoLists.Commands.UpdateTodoList;

public record UpdateTodoListCommand : IRequest
{
    public int Id { get; init; }
    public string? Title { get; init; }
    public Colour? Colour { get; init; }
}

public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand>
{
    private readonly IWriteRepository<TodoList> _repository;

    public UpdateTodoListCommandHandler(IWriteRepository<TodoList> repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.Id);
        Guard.Against.Null(request.Title);
        
        var entity = await _repository.GetByIdAsync(request.Id.Value, cancellationToken);
        Guard.Against.NotFound(request.Id.Value, entity);

        entity.Title = request.Title;
        
        if (request.Colour is not null)
        {
            entity.SetColour(request.Colour);
        }

        _repository.Update(entity);
        await _repository.SaveAsync(cancellationToken);
    }
}
