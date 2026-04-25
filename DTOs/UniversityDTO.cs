using System.ComponentModel.DataAnnotations;

namespace UniveersityApi.DTOs;

public class DepartmentDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string Name { get; set; }
}

public class CourseCreateDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string Title { get; set; }

    [Range(1, 5, ErrorMessage = "Credits must be between 1 and 5")]
    public int Credits { get; set; }

    [Required]
    public int DepartmentId { get; set; }
}

public class StudentCreateDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string Name { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}