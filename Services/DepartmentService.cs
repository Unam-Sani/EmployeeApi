using AutoMapper;
using EmployeeApi.DTOs.Departments;
using EmployeeApi.Exceptions;
using EmployeeApi.Models;
using EmployeeApi.Repositories.Interfaces;
using EmployeeApi.Services.Interfaces;

namespace EmployeeApi.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;

    public DepartmentService(
        IDepartmentRepository departmentRepository,
        IMapper mapper)
    {
        _departmentRepository = departmentRepository;
        _mapper = mapper;
    }

    public async Task<List<DepartmentReadDto>> GetAllAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return _mapper.Map<List<DepartmentReadDto>>(departments);
    }

    public async Task<DepartmentReadDto> GetByIdAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);

        if (department == null)
            throw new NotFoundException($"Department with ID {id} not found.");

        return _mapper.Map<DepartmentReadDto>(department);
    }

    public async Task<DepartmentReadDto> CreateAsync(DepartmentCreateDto dto)
    {
        var department = _mapper.Map<Department>(dto);

        await _departmentRepository.AddAsync(department);
        await _departmentRepository.SaveChangesAsync();

        return _mapper.Map<DepartmentReadDto>(department);
    }

    public async Task<DepartmentReadDto> UpdateAsync(int id, DepartmentUpdateDto dto)
    {
        var department = await _departmentRepository.GetByIdAsync(id);

        if (department == null)
            throw new NotFoundException($"Department with ID {id} not found.");

        _mapper.Map(dto, department);

        await _departmentRepository.UpdateAsync(department);
        await _departmentRepository.SaveChangesAsync();

        return _mapper.Map<DepartmentReadDto>(department);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);

        if (department == null)
            throw new NotFoundException($"Department with ID {id} not found.");

        await _departmentRepository.DeleteAsync(department);
        await _departmentRepository.SaveChangesAsync();

        return true;
    }
}
