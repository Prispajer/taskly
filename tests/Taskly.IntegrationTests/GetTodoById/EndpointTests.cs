using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Taskly.Application.Todos.GetTodoById;

namespace Taskly.Tests.Endpoints.Todos;

public class GetTodoByIdEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public GetTodoByIdEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_Return_200_When_Todo_Exists()
    {
        // Arrange: Create a valid todo
        var createRequest = new
        {
            Title = "Test Todo",
            Description = "Test Description",
            Expiry = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd")
        };

        var createResponse = await _client.PostAsJsonAsync("/todos", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await createResponse.Content.ReadFromJsonAsync<CreatedResponse>();
        created.Should().NotBeNull();
        created!.Id.Should().NotBe(Guid.Empty);

        // Act: Request the created todo by ID
        var getResponse = await _client.GetAsync($"/todos/{created.Id}");

        // Assert: Expect 200 OK and valid todo data
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var todo = await getResponse.Content.ReadFromJsonAsync<TodoResponse>();
        todo.Should().NotBeNull();
        todo!.Id.Should().Be(created.Id);
        todo.Title.Should().Be("Test Todo");
    }

    [Fact]
    public async Task Should_Return_404_When_Todo_Not_Found()
    {
        // Act: Request a non-existent todo
        var response = await _client.GetAsync($"/todos/{Guid.NewGuid()}");

        // Assert: Expect 404 NotFound with error details
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await response.Content.ReadFromJsonAsync<TestProblemDetails>();
        problem.Should().NotBeNull();
        problem!.Title.Should().Be("Todo.NotFound");
        problem.Status.Should().Be(404);
    }

    [Fact]
    public async Task Should_Return_400_When_Id_Is_Empty()
    {
        // Act: Send request with empty Guid
        var response = await _client.GetAsync("/todos/00000000-0000-0000-0000-000000000000");

        // Assert: Expect 400 BadRequest due to validation
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var raw = await response.Content.ReadAsStringAsync();
        (raw.Contains("validation") || raw.Contains("invalid") || raw.Contains("Todo Id"))
            .Should().BeTrue("the response should contain a validation-related message");
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

    private class TodoResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Expiry { get; set; }
        public int PercentComplete { get; set; }
    }
}
