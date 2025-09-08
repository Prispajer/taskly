using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace Taskly.IntegrationTests.Endpoints.Todos;

public class GetTodosEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public GetTodosEndpointTests(WebApplicationFactory<Program> factory)
    {
        // Create an HTTP client to interact with the test server
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_Return_200_And_Include_Created_Todo()
    {
        // Arrange: Send a POST request to create a new todo
        var createRequest = new
        {
            Title = "Test Todo",
            Description = "Test Description",
            Expiry = DateTime.UtcNow.AddDays(1)
        };

        var createResponse = await _client.PostAsJsonAsync("/todos", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Act: Send a GET request to retrieve all todos
        var getResponse = await _client.GetAsync("/todos");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Assert: The response contains the newly created todo
        var todos = await getResponse.Content.ReadFromJsonAsync<List<TodoResponse>>();
        todos.Should().NotBeNull();
        todos!.Any(t => t.Title == "Test Todo").Should().BeTrue();
    }

    [Fact]
    public async Task Should_Return_200_And_Contain_Valid_Todo_Objects()
    {
        // Act: Send a GET request to retrieve all todos
        var response = await _client.GetAsync("/todos");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Assert: All returned todos have valid fields
        var todos = await response.Content.ReadFromJsonAsync<List<TodoResponse>>();
        todos.Should().NotBeNull();

        foreach (var todo in todos!)
        {
            todo.Title.Should().NotBeNullOrEmpty();
            todo.Description.Should().NotBeNull();
            todo.Expiry.Should().BeAfter(DateTime.MinValue);
        }
    }

    // DTO used to deserialize the response from /todos
    private class TodoResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Expiry { get; set; }
        public int PercentComplete { get; set; }
    }
}

