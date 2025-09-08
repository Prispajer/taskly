using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace Taskly.IntegrationTests.Endpoints.Todos;

public class SetTodoPercentCompleteEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SetTodoPercentCompleteEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_Return_204_When_Percent_Is_Updated()
    {
        // Arrange: Create a new todo item
        var createRequest = new
        {
            Title = "Update me",
            Description = "Integration test",
            Expiry = DateTime.UtcNow.AddDays(1)
        };

        var createResponse = await _client.PostAsJsonAsync("/todos", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var location = createResponse.Headers.Location!.ToString();
        var id = location.Split("/").Last();

        // Act: Send PUT request to update percent
        var updateRequest = new { PercentComplete = 85 };
        var updateResponse = await _client.PutAsJsonAsync($"/todos/{id}/percent-complete", updateRequest);

        // Assert: Should return 204 No Content
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_Return_404_When_Todo_Does_Not_Exist()
    {
        // Act: Try to update a non-existent todo
        var updateRequest = new { PercentComplete = 50 };
        var response = await _client.PutAsJsonAsync($"/todos/{Guid.NewGuid()}/percent-complete", updateRequest);

        // Assert: Should return 404 Not Found
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_Return_400_When_Percent_Is_Invalid()
    {
        // Act: Send invalid percent value
        var updateRequest = new { PercentComplete = 999 };
        var response = await _client.PutAsJsonAsync($"/todos/{Guid.NewGuid()}/percent-complete", updateRequest);

        // Assert: Should return 400 Bad Request
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
