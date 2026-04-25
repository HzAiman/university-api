using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApi.Models;
using UniversityApi.Data;
using UniversityApi.DTOs;

namespace UniversityApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CoursesController : ControllerBase
{
    private readonly UniversityContext _context;

    public CoursesController(UniversityContext context)
    {
        _context = context;
    }

    // Create a new course
    [HttpPost]
    public async Task<ActionResult<Course>> PostCourse(CourseCreateDto courseDto)
    {
        // Map DTO to Model
        var course = new Course
        {
            Title = courseDto.Title,
            Credits = courseDto.Credits,
            DepartmentId = courseDto.DepartmentId.GetValueOrDefault()
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
    }

    // Get a course by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<CourseReadDto>> GetCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
        {
            return NotFound();
        }
        return Ok(new CourseReadDto
        {
            Id = course.Id,
            Title = course.Title,
            Credits = course.Credits,
            DepartmentId = course.DepartmentId
        });
    }

    // Filter courses
    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<CourseReadDto>>> FilterCourses(
        [FromQuery] string? title,
        [FromQuery] int? minCredits,
        [FromQuery] int? maxCredits)
    {
        var query = _context.Courses.AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(c => c.Title.Contains(title));
        }

        if (minCredits.HasValue)
        {
            query = query.Where(c => c.Credits >= minCredits.Value);
        }

        if (maxCredits.HasValue)
        {
            query = query.Where(c => c.Credits <= maxCredits.Value);
        }

        return await query.Select(c => new CourseReadDto
        {
            Id = c.Id,
            Title = c.Title,
            Credits = c.Credits,
            DepartmentId = c.DepartmentId
        }).ToListAsync();
    }
}