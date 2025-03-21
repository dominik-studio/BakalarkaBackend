using System.Text.Json.Serialization;
using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.Entities;
using PromobayBackend.Domain.Enums;
using PromobayBackend.Domain.ValueObjects;
using PromobayBackend.Application.Common.Interfaces;
using PromobayBackend.Domain.Common;

namespace PromobayBackend.Application.TodoLists.Commands.CreateTodoList;

public record CreateTodoListCommand : IRequest<int>
{
    public required string Title { get; init; }
    
    [JsonConverter(typeof(OptionalConverter<string?>))]
    public Optional<string?> Description { get; set; }
    public Colour? Colour { get; init; }
    public int? MaxItems { get; init; }
}

public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, int>
{
    private readonly IWriteRepository<TodoList> _repository;

    public CreateTodoListCommandHandler(IWriteRepository<TodoList> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoList
        {
            Title = request.Title
        };
        
        if (request.Description.HasValue)
            entity.SetDescription(request.Description.Value);

        if (request.Colour is not null)
            entity.SetColour(request.Colour);

        if (request.MaxItems.HasValue)
            entity.SetMaxItems(request.MaxItems.Value);

        _repository.Add(entity);
        await _repository.SaveAsync(cancellationToken);
        
        return entity.Id;
    }
}
