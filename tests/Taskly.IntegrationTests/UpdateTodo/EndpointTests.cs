using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace Taskly.IntegrationTests.Endpoints.Todos;

public class UpdateTodoEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UpdateTodoEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_Return_204_When_Todo_Is_Updated()
    {
        // Arrange: Create a new todo item
        var createRequest = new
        {
            Title = "Initial title",
            Description = "Initial description",
            Expiry = DateTime.UtcNow.AddDays(1).ToString("o")
        };

        var createResponse = await _client.PostAsJsonAsync("/todos", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var location = createResponse.Headers.Location!.ToString();
        var id = location.Split("/").Last();

        // Act: Send PUT request to update the todo
        var updateRequest = new
        {
            Title = "Updated title",
            Description = "Updated description",
            Expiry = DateTime.UtcNow.AddDays(2).ToString("o")
        };

        var updateResponse = await _client.PutAsJsonAsync($"/todos/{id}", updateRequest);

        // Assert: Should return 204 No Content
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_Return_404_When_Todo_Does_Not_Exist()
    {
        // Arrange: Prepare update request
        var updateRequest = new
        {
            Title = "Updated title",
            Description = "Updated description",
            Expiry = DateTime.UtcNow.AddDays(2).ToString("o")
        };

        // Act: Send PUT request with random ID
        var response = await _client.PutAsJsonAsync($"/todos/{Guid.NewGuid()}", updateRequest);

        // Assert: Should return 404 Not Found
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_Return_400_When_Data_Is_Invalid()
    {
        // Arrange: Prepare invalid update request
        var updateRequest = new
        {
            Title = "Short",
            Description = "Bad",
            Expiry = "invalid-date"
        };

        // Act: Send PUT request with invalid data
        var response = await _client.PutAsJsonAsync($"/todos/{Guid.NewGuid()}", updateRequest);

        // Assert: Should return 400 Bad Request
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
