using AutoMapper;
using EmployeeApi.DTOs.Employees;
using EmployeeApi.DTOs.Departments;
using EmployeeApi.Models;

namespace EmployeeApi.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Employee mappings
        CreateMap<EmployeeCreateDto, Employee>();
        CreateMap<EmployeeUpdateDto, Employee>();
        CreateMap<Employee, EmployeeReadDto>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))//src: This represents the Source (Employee).
            .ForMember(dest => dest.DepartmentName,                         //opt: configuration tool
                opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty));
//WHY 2nd ForMember If an employee is a new hire and hasn't been assigned a Department yet, src.Department will be null.

        // Department mappings
        CreateMap<DepartmentCreateDto, Department>();
        CreateMap<DepartmentUpdateDto, Department>();
        CreateMap<Department, DepartmentReadDto>();
    }
}
/*
- AutoMapper is like a Translator. It takes data from one object (the Source) and copies it into another object (the Destination).
- Why: Without this, you would have to manually write: 
    dto.FullName = emp.FirstName + " " + emp.LastName; over and over again. AutoMapper automates this "copy-pasting."
The "Custom" Map (The Complex Part):
- This is transformation, 
  Why:Your database has FirstName and LastName as separate columns. But your user wants a single FullName
- The ForMember method is how you tell AutoMapper to do something special for a specific property.
- opt.MapFrom(src => $"{src.FirstName} {src.LastName}") is a lambda expression that says:
 "When you are filling the FullName property, take the source object (which is an Employee), grab its FirstName and LastName, and concatenate them with a space in between."

*/