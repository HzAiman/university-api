using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApi.Models;
using UniversityApi.Data;
using UniversityApi.DTOs;
using AutoMapper;

namespace UniversityApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class StudentsController : ControllerBase
{
    private readonly UniversityContext _context;
    private readonly IMapper _mapper;

    public StudentsController(UniversityContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Enroll a student in a course
    [HttpPost("{studentId}/enroll/{courseId}")]
    public async Task<ActionResult> EnrollStudent(int studentId, int courseId)
    {
        // Verify student and course exist
        var student = await _context.Students.FindAsync(studentId);
        var course = await _context.Courses.FindAsync(courseId);

        if (student == null || course == null)
        {
            return NotFound("Student or Course not found.");
        }

        // Check if already enrolled
        var alreadyEnrolled = await _context.Enrollments
            .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        
        if (alreadyEnrolled)
        {
            return BadRequest("Student is already enrolled in this course.");
        }

        // Create the enrollment
        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return Ok($"Student {student.Name} enrolled in course {course.Title} successfully.");
    }

    // Get student schedule
    [HttpGet("{studentId}/schedule")]
    public async Task<ActionResult> GetStudentSchedule(int studentId)
    {
        var student = await _context.Students
            .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course) // Go through enrollments to get courses
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
        {
            return NotFound();
        }

        // Project to clean list of course title
        var schedule = student.Enrollments.Select(e => new
        {
            e.Course?.Title,
            e.EnrollmentDate
        });

        return Ok(new { student.Name, Courses = schedule});
    }

    // Create a new student
    [HttpPost]
    public async Task<ActionResult<StudentReadDto>> PostStudent(StudentCreateDto studentDto)
    {
        // Check if email already exists
        var emailExists = await _context.Students.AnyAsync(s => s.Email == studentDto.Email);
        if (emailExists)
        {
            return BadRequest("Email already in use.");
        }

        // Map DTO to Model
        var student = _mapper.Map<Student>(studentDto);

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, _mapper.Map<StudentReadDto>(student));
    }

    // Get a student by ID (used for CreatedAtAction in PostStudent)
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentReadDto>> GetStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<StudentReadDto>(student));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateStudent(int id, StudentUpdateDto studentDto)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        _mapper.Map(studentDto, student);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}