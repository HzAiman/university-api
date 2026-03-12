namespace UniversityApi.Models;

public class Department
{
    public int Id { get; set; }
    public required string Name { get; set; }

    // A department can have multiple courses
    public List<Course> Courses { get; set; } = new();
}