using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;

namespace McpFunc.McpTools;

public sealed class StudentQuestionsMcpTools
{
    private readonly StudentDirectoryClient _studentDirectoryClient;

    public StudentQuestionsMcpTools(StudentDirectoryClient studentDirectoryClient)
    {
        _studentDirectoryClient = studentDirectoryClient;
    }

    [Function(McpToolDefinitions.StudentCountTool.Name)]
    public async Task<StudentCountResult> GetStudentCount(
        [McpToolTrigger(
            McpToolDefinitions.StudentCountTool.Name,
            McpToolDefinitions.StudentCountTool.Description)]
        ToolInvocationContext context,
        CancellationToken cancellationToken)
    {
        var students = await _studentDirectoryClient.GetStudentsAsync(cancellationToken);
        return new StudentCountResult(students.Count);
    }

    [Function(McpToolDefinitions.YoungestStudentTool.Name)]
    public async Task<YoungestStudentsResult> GetYoungestStudent(
        [McpToolTrigger(
            McpToolDefinitions.YoungestStudentTool.Name,
            McpToolDefinitions.YoungestStudentTool.Description)]
        ToolInvocationContext context,
        CancellationToken cancellationToken)
    {
        var students = await _studentDirectoryClient.GetStudentsAsync(cancellationToken);
        if (students.Count == 0)
        {
            throw new InvalidDataException("The student data endpoint returned no students.");
        }

        var youngestDateOfBirth = students.Max(student => student.DateOfBirth);
        var youngestStudents = students
            .Where(student => student.DateOfBirth == youngestDateOfBirth)
            .ToArray();

        return new YoungestStudentsResult(youngestStudents);
    }

    [Function(McpToolDefinitions.StudentEnrollmentTool.Name)]
    public async Task<StudentEnrollmentResult> GetStudentEnrollment(
        [McpToolTrigger(
            McpToolDefinitions.StudentEnrollmentTool.Name,
            McpToolDefinitions.StudentEnrollmentTool.Description)]
        ToolInvocationContext context,
        CancellationToken cancellationToken)
    {
        var students = await _studentDirectoryClient.GetStudentsAsync(cancellationToken);
        var schoolCounts = students
            .GroupBy(student => student.School)
            .OrderBy(group => group.Key)
            .Select(group => new StudentGroupCount(group.Key, group.Count()))
            .ToArray();
        var genderCounts = students
            .GroupBy(student => student.Gender)
            .OrderBy(group => group.Key)
            .Select(group => new StudentGroupCount(group.Key, group.Count()))
            .ToArray();

        return new StudentEnrollmentResult(schoolCounts, genderCounts);
    }
}

public sealed record StudentCountResult(int TotalStudents);

public sealed record YoungestStudentsResult(IReadOnlyList<Student> Students);

public sealed record StudentGroupCount(string Group, int Count);

public sealed record StudentEnrollmentResult(
    IReadOnlyList<StudentGroupCount> BySchool,
    IReadOnlyList<StudentGroupCount> ByGender);
