using CRMBackend.Application.Common.Interfaces.Data;
using CRMBackend.Domain.Entities;
using CRMBackend.Domain.Enums;

namespace CRMBackend.Application.TodoLists.Commands.AddTodoItem
{
    public class AddTodoItemCommand : IRequest<int>
    {
        public int ListId { get; init; }
        public string? Title { get; init; }
        public string? Note { get; init; }
        public PriorityLevel Priority { get; init; }
        public DateTime Reminder { get; init; }
    }

    public class AddTodoItemCommandHandler : IRequestHandler<AddTodoItemCommand, int>
    {
        private readonly IWriteRepository<TodoList> _repository;

        public AddTodoItemCommandHandler(IWriteRepository<TodoList> repository) => _repository = repository;

        public async Task<int> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _repository.GetByIdAsync(request.ListId, cancellationToken);
            Guard.Against.NotFound(request.ListId, todoList);

            var item = new TodoItem
            {
                Title = request.Title,
                Note = request.Note,
                Priority = request.Priority,
                Reminder = request.Reminder
            };

            todoList.AddItem(item);
            await _repository.SaveAsync(cancellationToken);

            return item.Id;
        }
    }
} 
