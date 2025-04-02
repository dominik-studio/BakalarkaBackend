using CRMBackend.Application.Common.Interfaces.Repositories;

namespace CRMBackend.Application.DodavatelAggregate.Commands.Tovary.UpdateTovarAktivny
{
    public record UpdateTovarAktivnyCommand : IRequest
    {
        public required int DodavatelId { get; init; }
        public required int TovarId { get; init; }
        public required bool Aktivny { get; init; }
    }

    public class UpdateTovarAktivnyCommandHandler : IRequestHandler<UpdateTovarAktivnyCommand>
    {
        private readonly IWriteRepository<Domain.AggregateRoots.DodavatelAggregate.Dodavatel> _dodavatelRepository;

        public UpdateTovarAktivnyCommandHandler(IWriteRepository<Domain.AggregateRoots.DodavatelAggregate.Dodavatel> dodavatelRepository)
        {
            _dodavatelRepository = dodavatelRepository;
        }

        public async Task Handle(UpdateTovarAktivnyCommand request, CancellationToken cancellationToken)
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
