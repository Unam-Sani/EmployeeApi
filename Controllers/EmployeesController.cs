using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeApi.Services.Interfaces;
using EmployeeApi.DTOs.Employees;

namespace EmployeeApi.Controllers;

[Authorize] // 🔒 All endpoints require JWT
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    // GET: api/employees
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _employeeService.GetAllAsync();
        return Ok(employees);
    }

    // GET: api/employees/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        return Ok(employee);
    }

    // GET: api/employees/filter
    [HttpGet("filter")]
    public async Task<IActionResult> GetFiltered(
        [FromQuery] int? departmentId,
        [FromQuery] string? search)
    {
        var result = await _employeeService.GetFilteredAsync(departmentId, search);
        return Ok(result);
    }

    // POST: api/employees
    [HttpPost]
    public async Task<IActionResult> Create(EmployeeCreateDto dto)
    {
        var created = await _employeeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/employees/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, EmployeeUpdateDto dto)
    {
        var updated = await _employeeService.UpdateAsync(id, dto);
        return Ok(updated);
    }

    // DELETE: api/employees/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _employeeService.DeleteAsync(id);
        return NoContent();
    }
}
