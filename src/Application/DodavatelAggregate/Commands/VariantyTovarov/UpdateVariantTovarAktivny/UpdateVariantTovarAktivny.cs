using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Domain.AggregateRoots.TovarAggregate;

namespace CRMBackend.Application.DodavatelAggregate.Commands.VariantyTovarov.UpdateVariantTovarAktivny
{
    public record UpdateVariantTovarAktivnyCommand : IRequest
    {
        public required int TovarId { get; init; }
        public required int VariantId { get; init; }
        public required bool Aktivny { get; init; }
    }

    public class UpdateVariantTovarAktivnyCommandHandler : IRequestHandler<UpdateVariantTovarAktivnyCommand>
    {
        private readonly IWriteRepository<Tovar> _repository;

        public UpdateVariantTovarAktivnyCommandHandler(IWriteRepository<Tovar> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateVariantTovarAktivnyCommand request, CancellationToken cancellationToken)
        {
            var tovar = await _repository.GetByIdAsync(request.TovarId, cancellationToken);
            Guard.Against.NotFound(request.TovarId, tovar);
            
            var variant = tovar.Varianty.FirstOrDefault(v => v.Id == request.VariantId);
            Guard.Against.NotFound(request.VariantId, variant);
            
            variant.Aktivny = request.Aktivny;
            _repository.Update(tovar);
            await _repository.SaveAsync(cancellationToken);
        }
    }
} 
