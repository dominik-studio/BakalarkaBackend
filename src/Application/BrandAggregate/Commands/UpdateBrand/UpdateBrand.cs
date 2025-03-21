using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.AggregateRoots.BrandAggregate;

namespace PromobayBackend.Application.BrandAggregate.Commands.UpdateBrand;

public record UpdateBrandCommand : IRequest
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? LogoUrl { get; init; }
    public string? WebsiteUrl { get; init; }
}

public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand>
{
    private readonly IWriteRepository<Brand> _repository;

    public UpdateBrandCommandHandler(IWriteRepository<Brand> repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.Id);
        Guard.Against.Null(request.Name);
        
        var entity = await _repository.GetByIdAsync(request.Id.Value, cancellationToken);
        Guard.Against.NotFound(request.Id.Value, entity);

        entity.Name = request.Name;
        
        if (request.Description is not null)
        {
            entity.SetDescription(request.Description);
        }

        if (request.LogoUrl is not null)
        {
            entity.SetLogoUrl(request.LogoUrl);
        }

        if (request.WebsiteUrl is not null)
        {
            entity.SetWebsiteUrl(request.WebsiteUrl);
        }

        _repository.Update(entity);
        await _repository.SaveAsync(cancellationToken);
    }
} 
