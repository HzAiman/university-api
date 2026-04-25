using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApi.Models;
using UniversityApi.Data;
using UniversityApi.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace UniversityApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CoursesController : ControllerBase
{
    private readonly UniversityContext _context;
    private readonly IMapper _mapper;

    public CoursesController(UniversityContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Create a new course
    [HttpPost]
    public async Task<ActionResult<CourseReadDto>> PostCourse(CourseCreateDto courseDto)
    {
        var departmentExists = await _context.Departments.AnyAsync(d => d.Id == courseDto.DepartmentId);

        if (!departmentExists)
        {
            return BadRequest("Invalid department ID.");
        }
        // Map DTO to Model
        var course = _mapper.Map<Course>(courseDto);

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, _mapper.Map<CourseReadDto>(course));
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
        return Ok(_mapper.Map<CourseReadDto>(course));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCourse(int id, CourseUpdateDto courseDto)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        var departmentExists = await _context.Departments.AnyAsync(d => d.Id == courseDto.DepartmentId);
        if (!departmentExists)
        {
            return BadRequest("Target department does not exist.");
        }

        _mapper.Map(courseDto, course);
        await _context.SaveChangesAsync();

        return NoContent();
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

        return await query.ProjectTo<CourseReadDto>(_mapper.ConfigurationProvider).ToListAsync();
    }
}