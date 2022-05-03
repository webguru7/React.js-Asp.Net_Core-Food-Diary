using System;
using System.Net.Http.Json;
using FluentAssertions;
using FoodDiary.Application.Services.Export;
using FoodDiary.IntegrationTests.Fakes;
using Xunit;

namespace FoodDiary.IntegrationTests.Scenarios.Export;

public class ExportToGoogleDocsTests : IClassFixture<FoodDiaryWebApplicationFactory>
{
    private readonly FoodDiaryWebApplicationFactory _factory;

    public ExportToGoogleDocsTests(FoodDiaryWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async void Export_data_is_saved_to_google_drive_folder_as_google_doc()
    {
        var client = _factory.CreateClient();
        var googleDriveClient = _factory.GetGoogleDriveClient();
        var googleDocsClient = _factory.GetGoogleDocsClient();

        var exportRequest = new ExportToGoogleDocsRequestDto
        {
            StartDate = DateTime.Parse("2022-05-01"),
            EndDate = DateTime.Parse("2022-05-11"),
            AccessToken = "test"
        };
        
        var response = await client.PostAsJsonAsync("api/v1/exports/google-docs", exportRequest);
        var exportFile = await googleDriveClient.GetFileAsync(FakeGoogleDocsClient.NewDocId, exportRequest.AccessToken, default);
        var exportDocument = await googleDocsClient.GetDocumentAsync(exportFile?.Id, exportRequest.AccessToken, default);

        response.IsSuccessStatusCode.Should().BeTrue();
        exportFile.Should().NotBeNull();
        exportFile?.Name.Should().Be("FoodDiary_20220501_20220511");
        exportDocument.Should().NotBeNull();
    }
}