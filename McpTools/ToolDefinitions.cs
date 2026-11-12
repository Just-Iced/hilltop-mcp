namespace McpFunc.McpTools;

public static class McpToolDefinitions {

  public static class StudentCountTool
  {
    public const string Name = "GetStudentCount";
    public const string Description = "Returns the number of students in the student dataset.";
  }

  public static class YoungestStudentTool
  {
    public const string Name = "GetYoungestStudent";
    public const string Description = "Returns the youngest student or students, based on date of birth.";
  }

  public static class StudentEnrollmentTool
  {
    public const string Name = "GetStudentEnrollment";
    public const string Description = "Returns student counts grouped by school and gender.";
  }

  public static class DataTypes {
    public const string Number = "number";
    public const string String = "string";
  }
}