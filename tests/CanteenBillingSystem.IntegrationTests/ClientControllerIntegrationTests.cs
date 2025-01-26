using System.Net;
using System.Text;
using System.Text.Json;
using CanteenBillingSystem.API.Controllers.v1;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using CanteenBillingSystem.API;

namespace CanteenBillingSystem.IntegrationTests;

public class ClientControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ClientControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreditAccount_ShouldReturnOk_WhenValidRequest()
    {
        // Arrange
        var clientId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var request = new
        {
            Amount = 100m
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync($"/api/v1.0/clients/{clientId}/credit", jsonContent);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<CreditClientAccountResponse>(responseContent, options);

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal("Account credited successfully.", result.Message);
    }

    [Fact]
    public async Task CreditAccount_ShouldReturnNotFound_WhenClientDoesNotExist()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var request = new
        {
            Amount = 100m
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync($"/api/v1.0/clients/{clientId}/credit", jsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Contains("Client with ID", responseContent);
        Assert.Contains("not found", responseContent);
    }

    [Fact]
    public async Task CreditAccount_ShouldReturnBadRequest_WhenInvalidAmount()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var request = new
        {
            Amount = -50m
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync($"/api/v1.0/clients/{clientId}/credit", jsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Contains("Amount must be greater than zero", responseContent);
    }
}