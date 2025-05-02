using CRMBackend.Application.Common.DTOs.Invoices;
using CRMBackend.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using System.Reflection;
using System.Text;
using PuppeteerSharp.Media;
using RazorEngine;
using RazorEngine.Templating;

namespace CRMBackend.Infrastructure.Services.Invoices;

public class InvoiceGeneratorService : IInvoiceGeneratorService, IAsyncDisposable
{
    private readonly ILogger<InvoiceGeneratorService> _logger;
    private IBrowser? _browser;
    private bool _initialized = false;
    private bool _disposed = false;
    private readonly SemaphoreSlim _initializationLock = new(1, 1);

    public InvoiceGeneratorService(ILogger<InvoiceGeneratorService> logger)
    {
        _logger = logger;
    }

    private async Task EnsureInitializedAsync()
    {
        if (_initialized)
            return;

        await _initializationLock.WaitAsync();

        try
        {
            if (_initialized)
                return;

            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            _initialized = true;
            _logger.LogInformation("Puppeteer browser initialized for invoice generation");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize Puppeteer for invoice generation");
            throw;
        }
        finally
        {
            _initializationLock.Release();
        }
    }

    public async Task<byte[]> GenerateInvoicePdfAsync(InvoiceDTO invoiceData)
    {
        await EnsureInitializedAsync();

        if (_browser == null)
        {
            throw new InvalidOperationException("Puppeteer browser could not be initialized");
        }

        try
        {
            string templateHtml = await LoadTemplateAsync();
            string processedHtml = ProcessTemplate(templateHtml, invoiceData);
            
            using var page = await _browser.NewPageAsync();
            
            await page.SetContentAsync(processedHtml);
            await page.WaitForNetworkIdleAsync();

            var pdfBytes = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions
                {
                    Top = "20mm",
                    Bottom = "20mm",
                    Left = "10mm",
                    Right = "10mm"
                }
            });

            _logger.LogInformation("Generated invoice PDF for company {CompanyName}, invoice #{InvoiceNumber}",
                invoiceData.NazovFirmy, invoiceData.CisloDokumentu);
            
            return pdfBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating invoice PDF for company {CompanyName}", invoiceData.NazovFirmy);
            throw;
        }
    }

    private async Task<string> LoadTemplateAsync()
    {
        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
        string templatePath = Path.Combine(assemblyPath, "Services", "Invoices", "Templates", "InvoiceTemplate.html");
        
        if (!File.Exists(templatePath))
        {
            _logger.LogWarning("Invoice template not found at {TemplatePath}", templatePath);
            
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("CRMBackend.Infrastructure.Services.Invoices.Templates.InvoiceTemplate.html");
            
            if (stream == null)
            {
                throw new FileNotFoundException("Invoice template not found as embedded resource");
            }
            
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
        
        return await File.ReadAllTextAsync(templatePath);
    }

    private string ProcessTemplate(string templateHtml, InvoiceDTO invoiceData)
    {
        try
        {
            string key = $"invoice_{invoiceData.CisloDokumentu}";
            
            if (!Engine.Razor.IsTemplateCached(key, typeof(InvoiceDTO)))
            {
                Engine.Razor.Compile(templateHtml, key, typeof(InvoiceDTO));
            }
            
            return Engine.Razor.Run(key, typeof(InvoiceDTO), invoiceData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing invoice template");
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            if (_browser != null)
            {
                await _browser.CloseAsync();
                await _browser.DisposeAsync();
                _browser = null;
            }
            
            _initializationLock.Dispose();
            _disposed = true;
        }
        
        GC.SuppressFinalize(this);
    }

    ~InvoiceGeneratorService()
    {
        if (!_disposed)
        {
            _logger.LogWarning("InvoiceGeneratorService was not disposed properly");
        }
    }
} 
