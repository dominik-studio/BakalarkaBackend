namespace CRMBackend.Application.Common.Models;

public abstract class BaseAuditableDto
{
    public DateTimeOffset VytvoreneDna { get; set; }
    public string? VytvorilUzivatel { get; set; }
    public DateTimeOffset UpraveneDna { get; set; }
    public string? UpravilUzivatel { get; set; }
}
