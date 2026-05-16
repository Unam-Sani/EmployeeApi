using Microsoft.AspNetCore.Mvc;
using EmployeeApi.Services.Interfaces;
using EmployeeApi.DTOs.Departments;

namespace EmployeeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var departments = await _departmentService.GetAllAsync();
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        return Ok(department);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DepartmentCreateDto dto)
    {
        var department = await _departmentService.CreateAsync(dto);
        return Ok(department);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DepartmentUpdateDto dto)
    {
        var department = await _departmentService.UpdateAsync(id, dto);
        return Ok(department);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _departmentService.DeleteAsync(id);
        return Ok(result);
    }
}
