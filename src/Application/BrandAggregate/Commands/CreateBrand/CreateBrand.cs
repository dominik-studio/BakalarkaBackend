using System.Diagnostics;
using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.AggregateRoots.BrandAggregate;

namespace PromobayBackend.Application.BrandAggregate.Commands.CreateBrand;

public record CreateBrandCommand : IRequest<int>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? LogoUrl { get; init; }
    public string? WebsiteUrl { get; init; }
}

public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, int>
{
    private readonly IWriteRepository<Brand> _repository;

    public CreateBrandCommandHandler(IWriteRepository<Brand> repository) => _repository = repository;

    public async Task<int> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.Name);
        
        var entity = new Brand
        {
            Name = request.Name
        };

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

        _repository.Add(entity);
        await _repository.SaveAsync(cancellationToken);
        
        return entity.Id;
    }
} 
