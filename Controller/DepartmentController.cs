using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApi.Models;
using UniversityApi.Data;
using UniveersityApi.DTOs;

namespace UniversityApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class DepartmentController : ControllerBase
{
    private readonly UniversityContext _context;

    public DepartmentController(UniversityContext context)
    {
        _context = context;
    }

    // List all departments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartment()
    {
        return await _context.Departments.ToListAsync();
    }

    // Create a new department
    [HttpPost]
    public async Task<ActionResult<Department>> PostDepartment(DepartmentDto departmentDto)
    {
        var department = new Department
        {
            Name = departmentDto.Name
        };
        
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
    }

    // Get overall stats
    [HttpGet("stats")]
    public async Task<ActionResult> GetUniversityStats()
    {
        var stats = await _context.Departments
            .Select(d => new
            {
                DepartmentName = d.Name,
                CourseCount = d.Courses.Count(),
                TotalCredits = d.Courses.Sum(c => c.Credits),
                AverageCredits = d.Courses.Any() ? d.Courses.Average(c => c.Credits) : 0
            })
            .ToListAsync();

        return Ok(stats);
    }
}