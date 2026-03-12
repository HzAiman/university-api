namespace UniversityApi.Models;

public class Course
{
    public int Id { get; set; }
    public required string Title { get; set;}
    public int Credits { get; set; }

    // Foreign key to the Department
    public int DepartmentId { get; set; }

    // See the Department object from the Course
    public Department? Department { get; set; }
}