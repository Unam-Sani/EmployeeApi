using Microsoft.AspNetCore.Mvc;
using EmployeeApi.Services.Interfaces;
using EmployeeApi.DTOs.Departments;

namespace EmployeeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
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
}
