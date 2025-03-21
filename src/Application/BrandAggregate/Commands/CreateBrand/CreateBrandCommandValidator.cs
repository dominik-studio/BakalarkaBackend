using FluentValidation;
using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.AggregateRoots.BrandAggregate;

namespace PromobayBackend.Application.BrandAggregate.Commands.CreateBrand;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    private readonly IReadRepository<Brand> _repository;

    public CreateBrandCommandValidator(IReadRepository<Brand> repository)
    {
        _repository = repository;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MustAsync(BeUniqueTitle)
            .WithMessage("'{PropertyName}' must be unique.")
            .WithErrorCode("Unique");
        
        RuleFor(v => v.LogoUrl)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(v => !string.IsNullOrEmpty(v.LogoUrl))
            .WithMessage("LogoUrl must be a valid absolute URI.");
            
        RuleFor(v => v.WebsiteUrl)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(v => !string.IsNullOrEmpty(v.WebsiteUrl))
            .WithMessage("WebsiteUrl must be a valid absolute URI.");
    }

    private async Task<bool> BeUniqueTitle(string? title, CancellationToken cancellationToken)
    {
        return !await _repository.GetQueryableNoTracking()
            .AnyAsync(b => b.Name == title, cancellationToken);
    }
} 
