using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTOs;

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
    public int? DepartmentId { get; set; }
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

public class CourseReadDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int Credits { get; set; }
    public int DepartmentId { get; set; }
}

public class StudentReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class StudentUpdateDto
{
    [Required]
    public string Name { get; set; } = null!;
}