using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace McpFunc.McpTools;

public sealed class StudentDirectoryClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StudentDirectoryClient> _logger;

    public StudentDirectoryClient(HttpClient httpClient, ILogger<StudentDirectoryClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IReadOnlyList<Student>> GetStudentsAsync(CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync(string.Empty, cancellationToken);
        response.EnsureSuccessStatusCode();

        var students = await response.Content.ReadFromJsonAsync<Student[]>(
            cancellationToken: cancellationToken);

        if (students is null)
        {
            throw new InvalidDataException("The student data endpoint returned an empty response.");
        }

        _logger.LogInformation("Retrieved {StudentCount} students from the student data endpoint.", students.Length);
        return students;
    }
}

public sealed record Student(
    [property: JsonPropertyName("StudentId")] int StudentId,
    [property: JsonPropertyName("FirstName")] string FirstName,
    [property: JsonPropertyName("LastName")] string LastName,
    [property: JsonPropertyName("School")] string School,
    [property: JsonPropertyName("Gender")] string Gender,
    [property: JsonPropertyName("DateOfBirth")] DateOnly DateOfBirth);
