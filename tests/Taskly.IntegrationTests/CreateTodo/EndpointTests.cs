using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace Taskly.IntegrationTests.Endpoints.Todos;

public class CreateTodoEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CreateTodoEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_Return_201_When_Valid_Request()
    {
        // Arrange: Prepare a valid request payload
        var request = new
        {
            Title = "Valid Title",
            Description = "Valid Description",
            Expiry = DateTime.UtcNow.AddDays(1)
        };

        // Act: Send POST request to create a todo
        var response = await _client.PostAsJsonAsync("/todos", request);

        // Assert: Expect HTTP 201 Created
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Should_Return_400_When_Expiry_Format_Is_Invalid()
    {
        // Arrange: Prepare a request with invalid expiry format
        var request = new
        {
            Title = "Valid",
            Description = "Valid",
            Expiry = "not-a-date"
        };

        // Act: Send POST request
        var response = await _client.PostAsJsonAsync("/todos", request);

        // Assert: Expect HTTP 400 Bad Request with validation error details
        var problem = await response.Content.ReadFromJsonAsync<TestProblemDetails>();
        problem!.Title.Should().Be("One or more validation errors occurred.");
        problem.Status.Should().Be(400);
    }

    private class TestProblemDetails
    {
        public string? Title { get; set; }
        public string? Detail { get; set; }
        public int? Status { get; set; }
    }
}


