using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.AggregateRoots.BrandAggregate;

namespace PromobayBackend.Application.BrandAggregate.Commands.DeleteBrand;

public record DeleteBrandCommand(int Id) : IRequest;

public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand>
{
    private readonly IWriteRepository<Brand> _repository;

    public DeleteBrandCommandHandler(IWriteRepository<Brand> repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);

        _repository.Delete(entity);
        await _repository.SaveAsync(cancellationToken);
    }
} 