using CRMBackend.Application.Common.Interfaces.Repositories;
using CRMBackend.Domain.AggregateRoots.KategorieProduktovAggregate;

namespace CRMBackend.Application.DodavatelAggregate.Commands.Tovary.UpdateTovar
{
    public record UpdateTovarCommand : IRequest
    {
        public required int DodavatelId { get; init; }
        public required int TovarId { get; init; }
        public required string InterneId { get; init; }
        public required string Nazov { get; init; }
        public required int KategoriaId { get; init; }
        public required decimal Cena { get; init; }
        public string? ObrazokURL { get; init; }
        public string? Ean { get; init; }
    }

    public class UpdateTovarCommandHandler : IRequestHandler<UpdateTovarCommand>
    {
        private readonly IWriteRepository<Domain.AggregateRoots.DodavatelAggregate.Dodavatel> _dodavatelRepository;
        private readonly IWriteRepository<KategorieProduktov> _kategorieRepository;

        public UpdateTovarCommandHandler(IWriteRepository<Domain.AggregateRoots.DodavatelAggregate.Dodavatel> dodavatelRepository, IWriteRepository<KategorieProduktov> kategorieRepository)
        {
            _dodavatelRepository = dodavatelRepository;
            _kategorieRepository = kategorieRepository;
        }

        public async Task Handle(UpdateTovarCommand request, CancellationToken cancellationToken)
        {
            var dodavatel = await _dodavatelRepository.GetByIdAsync(request.DodavatelId, cancellationToken);
            Guard.Against.NotFound(request.DodavatelId, dodavatel);
            var tovar = dodavatel.Tovary.FirstOrDefault(t => t.Id == request.TovarId);
            Guard.Against.NotFound(request.TovarId, tovar);
            tovar.InterneId = request.InterneId;
            tovar.Nazov = request.Nazov;
            tovar.Cena = request.Cena;
            if (tovar.KategoriaId != request.KategoriaId)
            {
                var kategoria = await _kategorieRepository.GetByIdAsync(request.KategoriaId, cancellationToken);
                Guard.Against.NotFound(request.KategoriaId, kategoria);
                tovar.Kategoria = kategoria;
                tovar.KategoriaId = request.KategoriaId;
            }
            tovar.SetObrazok(request.ObrazokURL);
            tovar.SetEan(request.Ean);
            _dodavatelRepository.Update(dodavatel);
            await _dodavatelRepository.SaveAsync(cancellationToken);
        }
    }
} 
