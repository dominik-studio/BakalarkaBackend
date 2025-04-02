using CRMBackend.Application.Common.Interfaces.Repositories;

namespace CRMBackend.Application.FirmaAggregate.Commands.KontaktnaOsoba.UpdateKontaktnaOsoba
{
    public record UpdateKontaktnaOsobaCommand : IRequest
    {
        public required int FirmaId { get; init; }
        public required int KontaktnaOsobaId { get; init; }
        public required string Meno { get; init; }
        public required string Priezvisko { get; init; }
        public required string Telefon { get; init; }
        public required string Email { get; init; }
    }

    public class UpdateKontaktnaOsobaCommandHandler : IRequestHandler<UpdateKontaktnaOsobaCommand>
    {
        private readonly IWriteRepository<Domain.AggregateRoots.FirmaAggregate.Firma> _repository;

        public UpdateKontaktnaOsobaCommandHandler(IWriteRepository<Domain.AggregateRoots.FirmaAggregate.Firma> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateKontaktnaOsobaCommand request, CancellationToken cancellationToken)
        {
            var firma = await _repository.GetByIdAsync(request.FirmaId, cancellationToken);
            Guard.Against.NotFound(request.FirmaId, firma);
            var kontaktnaOsoba = firma.KontaktneOsoby.FirstOrDefault(o => o.Id == request.KontaktnaOsobaId);
            Guard.Against.NotFound(request.KontaktnaOsobaId, kontaktnaOsoba);
            kontaktnaOsoba.Meno = request.Meno;
            kontaktnaOsoba.Priezvisko = request.Priezvisko;
            kontaktnaOsoba.Telefon = request.Telefon;
            kontaktnaOsoba.Email = request.Email;
            _repository.Update(firma);
            await _repository.SaveAsync(cancellationToken);
        }
    }
} 
