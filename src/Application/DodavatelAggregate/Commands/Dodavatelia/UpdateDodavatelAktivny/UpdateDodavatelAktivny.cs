using CRMBackend.Application.Common.Interfaces.Repositories;

namespace CRMBackend.Application.DodavatelAggregate.Commands.Dodavatelia.UpdateDodavatelAktivny
{
    public record UpdateDodavatelAktivnyCommand : IRequest
    {
        public required int Id { get; init; }
        public required bool Aktivny { get; init; }
    }

    public class UpdateDodavatelAktivnyCommandHandler : IRequestHandler<UpdateDodavatelAktivnyCommand>
    {
        private readonly IWriteRepository<Domain.AggregateRoots.DodavatelAggregate.Dodavatel> _repository;
        public UpdateDodavatelAktivnyCommandHandler(IWriteRepository<Domain.AggregateRoots.DodavatelAggregate.Dodavatel> repository)
        {
            _repository = repository;
        }
        public async Task Handle(UpdateDodavatelAktivnyCommand request, CancellationToken cancellationToken)
        {
            var dodavatel = await _repository.GetByIdAsync(request.Id, cancellationToken);
            Guard.Against.NotFound(request.Id, dodavatel);
            dodavatel.Aktivny = request.Aktivny;
            _repository.Update(dodavatel);
            await _repository.SaveAsync(cancellationToken);
        }
    }
} 
