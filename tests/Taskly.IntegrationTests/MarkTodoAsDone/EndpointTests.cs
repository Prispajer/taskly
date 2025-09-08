using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace Taskly.IntegrationTests.Endpoints.Todos;

public class MarkTodoAsDoneEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public MarkTodoAsDoneEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_Return_204_When_Todo_Is_Marked_As_Done()
    {
        // Arrange: Create a new todo
        var createRequest = new
        {
            Title = "Complete Me",
            Description = "Test Test",
            Expiry = DateTime.UtcNow.AddDays(1)
        };

        var createResponse = await _client.PostAsJsonAsync("/todos", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var location = createResponse.Headers.Location!.ToString();
        var id = location.Split("/").Last();

        // Act: Send POST to mark as done
        var response = await _client.PostAsync($"/todos/{id}/completed", null);

        // Assert: Expect 204 No Content
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_Return_404_When_Todo_Does_Not_Exist()
    {
        // Act: Send POST with a non-existent GUID
        var response = await _client.PostAsync($"/todos/{Guid.NewGuid()}/completed", null);

        // Assert: Expect 404 Not Found
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
