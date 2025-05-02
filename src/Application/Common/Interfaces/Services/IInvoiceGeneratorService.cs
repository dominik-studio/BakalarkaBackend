using CRMBackend.Application.Common.DTOs.Invoices;

namespace CRMBackend.Application.Common.Interfaces.Services;

public interface IInvoiceGeneratorService
{
    /// <summary>
    /// Generates a PDF invoice from the provided invoice data
    /// </summary>
    /// <param name="invoiceData">The invoice data to generate PDF from</param>
    /// <returns>PDF document as byte array</returns>
    Task<byte[]> GenerateInvoicePdfAsync(InvoiceDTO invoiceData);
} 