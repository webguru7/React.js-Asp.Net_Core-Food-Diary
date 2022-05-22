using System.Threading;
using System.Threading.Tasks;
using FoodDiary.Application.Services.Export.DataLoader;
using FoodDiary.Export.GoogleDocs;

namespace FoodDiary.Application.Services.Export;

internal class ExportService : IExportService
{
    private readonly IExportDataLoader _dataLoader;
    private readonly IGoogleDocsExportService _googleDocsService;

    public ExportService(IExportDataLoader dataLoader, IGoogleDocsExportService googleDocsService)
    {
        _dataLoader = dataLoader;
        _googleDocsService = googleDocsService;
    }
    
    public async Task<ExportToGoogleDocsResponseDto> ExportToGoogleDocsAsync(ExportToGoogleDocsRequestDto request,
        CancellationToken cancellationToken)
    {
        var exportFileDto = await _dataLoader.GetExportDataAsync(request.StartDate,
            request.EndDate,
            cancellationToken);

        var documentId = await _googleDocsService.ExportAsync(exportFileDto, request.AccessToken, cancellationToken);

        return new ExportToGoogleDocsResponseDto
        {
            DocumentId = documentId
        };
    }

    public Task<byte[]> ExportToJsonAsync(ExportRequestDto request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}