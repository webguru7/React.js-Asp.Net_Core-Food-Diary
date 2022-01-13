using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using FoodDiary.Application.Features.Auth.SignInWithGoogle;
using FoodDiary.Contracts.Auth;
using Xunit;

namespace FoodDiary.IntegrationTests.Scenarios.Auth;

public class SignInWithGoogleTests : IClassFixture<FoodDiaryApplicationFactory>
{
    private readonly FoodDiaryApplicationFactory _applicationFactory = new();
    
    [Fact]
    public async Task Authenticates_whitelisted_user_with_valid_google_token_id_and_provides_access_token()
    {
        var client = _applicationFactory.CreateClient();
        var request = new SignInWithGoogleRequest
        {
            GoogleTokenId = "test_google_token_id"
        };
        
        var response = await client.PostAsJsonAsync("/api/v1/auth/google", request);
        var authResponseData = await response.Content.ReadFromJsonAsync<SuccessfulAuthResponseDto>();

        response.Should().Be(HttpStatusCode.OK);
        authResponseData.Should().NotBeNull();
        authResponseData?.AccessToken.Should().NotBeNull();
    }
}