using Application.DTOs.Request;
using Commons.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;

namespace WebAPI.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesContract _service;

        public EmployeesController(IEmployeesContract service) => this._service = service;

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            try
            {
                var employees = await this._service.GetEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception exception)
            {
                return exception.ConvertToActionResult(HttpContext);
            }
        }

        // GET: api/Employees/5
        [HttpGet("{EmployeeID}")]
        public async Task<ActionResult<Employee>> Get(int EmployeeID)
        {
            if (EmployeeID <= 0)
                return BadRequest("Invalid ID");

            var employee = await Task.Run(() => this._service.GetEmployeeByIdAsync(EmployeeID));

            if (employee == null)
                return NotFound();

            return employee;
        }

        // POST: api/Employees
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Employee>> Post([FromBody] EmployeeRequestDTO employeeRequest)
        {
            try
            {
                if (employeeRequest == null)
                    return BadRequest("Error when trying to create the employee");

                var employee = new Employee()
                {
                    FirstName = employeeRequest.FirstName,
                    LastName = employeeRequest.LastName,
                    Email = employeeRequest.Email,
                    Address = employeeRequest.Address,
                    Position = employeeRequest.Position,
                    Phone = employeeRequest.Phone
                };

                await this._service.CreateEmployeeAsync(employee);

                return StatusCode((int)HttpStatusCode.Created, employee);
            }
            catch (Exception exception)
            {
                return exception.ConvertToActionResult(HttpContext);
            }
        }

        // PUT: api/Employees        
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put([FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                    return BadRequest();

                await this._service.UpdateEmployeeAsync(employee);

                return Ok("Record successfully updated");
            }
            catch (Exception exception)
            {
                return exception.ConvertToActionResult(HttpContext);
            }
        }

        // DELETE: api/Employees/5
        [HttpDelete("{EmployeeID}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int EmployeeID)
        {
            try
            {
                if (EmployeeID <= 0)
                    return BadRequest("Invalid ID");

                var employee = this._service.GetEmployeeByIdAsync(EmployeeID);

                if (employee == null)
                    return NotFound();

                await this._service.DeleteEmployeeAsync(employee);

                return Ok("Record successfully deleted");
            }
            catch (Exception exception)
            {
                return exception.ConvertToActionResult(HttpContext);
            }
        }
    }
}
