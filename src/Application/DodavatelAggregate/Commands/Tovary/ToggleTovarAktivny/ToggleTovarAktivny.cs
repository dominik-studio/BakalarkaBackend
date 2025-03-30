using CRMBackend.Application.Common.Interfaces.Repositories;

namespace CRMBackend.Application.DodavatelAggregate.Commands.Tovary.ToggleTovarAktivny
{
    public record ToggleTovarAktivnyCommand : IRequest
    {
        public required int DodavatelId { get; init; }
        public required int TovarId { get; init; }
        public required bool Aktivny { get; init; }
    }

    public class ToggleTovarAktivnyCommandHandler : IRequestHandler<ToggleTovarAktivnyCommand>
    {
        private readonly IWriteRepository<Domain.AggregateRoots.DodavatelAggregate.Dodavatel> _dodavatelRepository;

        public ToggleTovarAktivnyCommandHandler(IWriteRepository<Domain.AggregateRoots.DodavatelAggregate.Dodavatel> dodavatelRepository)
        {
            _dodavatelRepository = dodavatelRepository;
        }

        public async Task Handle(ToggleTovarAktivnyCommand request, CancellationToken cancellationToken)
        {
            var dodavatel = await _dodavatelRepository.GetByIdAsync(request.DodavatelId, cancellationToken);
            Guard.Against.NotFound(request.DodavatelId, dodavatel);
            var tovar = dodavatel.Tovary.FirstOrDefault(t => t.Id == request.TovarId);
            Guard.Against.NotFound(request.TovarId, tovar);
            tovar.Aktivny = request.Aktivny;
            _dodavatelRepository.Update(dodavatel);
            await _dodavatelRepository.SaveAsync(cancellationToken);
        }
    }
} 
