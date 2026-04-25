using AutoMapper;
using UniversityApi.Models;
using UniversityApi.DTOs;

namespace UniversityApi.Profiles;

public class UniversityProfile : Profile
{
    public UniversityProfile()
    {
        CreateMap<Student, StudentReadDto>();
        CreateMap<StudentCreateDto, Student>();
        CreateMap<StudentUpdateDto, Student>();
        CreateMap<Course, CourseReadDto>();
        CreateMap<CourseCreateDto, Course>();
        CreateMap<CourseUpdateDto, Course>();
    }
}