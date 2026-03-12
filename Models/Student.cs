namespace UniversityApi.Models;

public class Student
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

    public List<Enrollment> Enrollments { get; set; } = new();
}