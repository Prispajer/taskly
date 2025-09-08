using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace Taskly.IntegrationTests.Endpoints.Todos;

public class DeleteTodoEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public DeleteTodoEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_Return_204_When_Todo_Deleted()
    {
        // Arrange: Prepare a valid todo creation request
        var createRequest = new
        {
            Title = "Valid title for deletion",
            Description = "This is a valid description for testing deletion.",
            Expiry = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd")
        };

        // Act: Create the todo
        var createResponse = await _client.PostAsJsonAsync("/todos", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await createResponse.Content.ReadFromJsonAsync<CreatedResponse>();
        created.Should().NotBeNull();
        created!.Id.Should().NotBe(Guid.Empty);

        // Act: Delete the created todo
        var deleteResponse = await _client.DeleteAsync($"/todos/{created.Id}");

        // Assert: Expect 204 NoContent
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_Return_404_When_Todo_Not_Found()
    {
        // Act: Attempt to delete a non-existent todo
        var response = await _client.DeleteAsync($"/todos/{Guid.NewGuid()}");

        // Assert: Expect 404 NotFound with proper error details
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await response.Content.ReadFromJsonAsync<TestProblemDetails>();
        problem.Should().NotBeNull();
        problem!.Title.Should().Be("Todo.NotFound");
        problem.Status.Should().Be(404);
    }

    private class CreatedResponse
    {
        public Guid Id { get; set; }
    }

    private class TestProblemDetails
    {
        public string Title { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
