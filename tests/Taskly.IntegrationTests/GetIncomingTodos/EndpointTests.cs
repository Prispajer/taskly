using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Taskly.Application.Todos.GetIncomingTodo;

namespace Taskly.Tests.Endpoints.Todos;

public class GetIncomingTodosEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public GetIncomingTodosEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("Today")]
    [InlineData("NextDay")]
    [InlineData("CurrentWeek")]
    public async Task Should_Return_200_With_Todos_For_Valid_Range(string range)
    {
        // Act: Send GET request with valid range
        var response = await _client.GetAsync($"/todos/incoming/{range}");

        // Assert: Expect 200 OK and a list of todos
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var todos = await response.Content.ReadFromJsonAsync<List<TodoResponse>>();
        todos.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_Return_400_When_Range_Is_Invalid()
    {
        // Act: Send GET request with invalid range
        var response = await _client.GetAsync("/todos/incoming/InvalidRange");

        // Assert: Expect 400 BadRequest
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Try to parse JSON only if content type is correct
        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            var problem = await response.Content.ReadFromJsonAsync<TestProblemDetails>();
            problem.Should().NotBeNull();
            problem!.Title.Should().Be("One or more validation errors occurred.");
            problem.Status.Should().Be(400);
        }
        else
        {
            // Fallback: check raw content if not JSON
            var raw = await response.Content.ReadAsStringAsync();
            (raw.Contains("validation") || raw.Contains("invalid") || raw.Contains("Range"))
                .Should().BeTrue("the response should contain a validation-related message");
        }
    }


    private class TestProblemDetails
    {
        public string Title { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
