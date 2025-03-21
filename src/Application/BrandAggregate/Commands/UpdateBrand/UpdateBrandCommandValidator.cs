using FluentValidation;
using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.AggregateRoots.BrandAggregate;

namespace PromobayBackend.Application.BrandAggregate.Commands.UpdateBrand;

public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    private readonly IReadRepository<Brand> _repository;

    public UpdateBrandCommandValidator(IReadRepository<Brand> repository)
    {
        _repository = repository;

        RuleFor(v => v.Id)
            .NotNull().WithMessage("Id is required.")
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(v => v.Name)
            .NotNull().WithMessage("Name is required.")
            .NotEmpty().WithMessage("Name is required.")
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

    private async Task<bool> BeUniqueTitle(UpdateBrandCommand command, string? title, CancellationToken cancellationToken)
    {
        return !await _repository.GetQueryableNoTracking()
            .AnyAsync(b => b.Name == title && b.Id != command.Id, cancellationToken);
    }
} 
