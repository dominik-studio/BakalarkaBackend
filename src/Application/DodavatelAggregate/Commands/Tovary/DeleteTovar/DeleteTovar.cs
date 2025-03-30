using CRMBackend.Application.Common.Interfaces.Repositories;

namespace CRMBackend.Application.DodavatelAggregate.Commands.Tovary.DeleteTovar
{
    public record DeleteTovarCommand : IRequest
    {
        public required int DodavatelId { get; init; }
        public required int TovarId { get; init; }
    }

    public class DeleteTovarCommandHandler : IRequestHandler<DeleteTovarCommand>
    {
        private readonly IWriteRepository<Domain.AggregateRoots.DodavatelAggregate.Dodavatel> _dodavatelRepository;

        public DeleteTovarCommandHandler(IWriteRepository<Domain.AggregateRoots.DodavatelAggregate.Dodavatel> dodavatelRepository)
        {
            _dodavatelRepository = dodavatelRepository;
        }

        public async Task Handle(DeleteTovarCommand request, CancellationToken cancellationToken)
        {
            var dodavatel = await _dodavatelRepository.GetByIdAsync(request.DodavatelId, cancellationToken);
            Guard.Against.NotFound(request.DodavatelId, dodavatel);
            var tovar = dodavatel.Tovary.FirstOrDefault(t => t.Id == request.TovarId);
            Guard.Against.NotFound(request.TovarId, tovar);
            dodavatel.RemoveTovar(request.TovarId);
            _dodavatelRepository.Update(dodavatel);
            await _dodavatelRepository.SaveAsync(cancellationToken);
        }
    }
} 
