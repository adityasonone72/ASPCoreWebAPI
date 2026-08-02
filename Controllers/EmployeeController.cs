using ASPCoreWebAPI.DTOs;
using ASPCoreWebAPI.Models;
using ASPCoreWebAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; //it is no longer needed, as we are not using Json.Serialize, Ok() uses System.Text.Json for serialization
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Runtime.InteropServices.Marshalling;

namespace ASPCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    /*[ApiController] enables API-specific behaviors. One of them is automatic model validation. 
     * If the model is invalid, ASP.NET Core automatically returns a 400 Bad Request response before the controller action executes. 
     * Without [ApiController], validation errors are stored in ModelState, and the developer must manually check ModelState.IsValid."*/

    //since we are using webapi, we don't need whole Controller, since WEBAPI never returns View()
    //so, we are using ControllerBase, as we don't need additional MVC views functionality.
    //Controller = ControllerBase + MVC View support.
    public class EmployeeController : ControllerBase
    {

        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        [HttpGet]
        //[Route("GetAllEmployees")]
        //[Obsolete]
        public async Task<ActionResult<List<Employee>>> GetEmployees(CancellationToken cancellationToken)
        {
            //Added Dependency Injection,  Now controller don't need to handle SQL connection, running queries, etc. 
            //EmployeeController now only has one job, which is to handle http request and response.

            //Task<List<Employee>> employees = _employeeRepository.GetEmployees();

            List<Employee> result = await _employeeRepository.GetEmployees(cancellationToken);
            return Ok(result); //creates object of OkObjectResult, which will give 200Ok statusCode, and uses System.Text.Json for 
            //return View();
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<Employee?>> GetEmployeeById(int Id, CancellationToken cancellationToken)
        {
            var result = await _employeeRepository.GetEmployeeById(Id, cancellationToken);

            if (result == null) {
                return NotFound("Employee Record Not found for this Id");
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(CreateEmployeeDto dto, CancellationToken cancellationToken)
        {
            //var result = await _employeeRepository.AddEmployee(employee, cancellationToken);

            ////if (result == null) { //it will never be  null, even if insertion failed.
            ////    return BadRequest("Insertion falied");
            ////}
            ////else
            ////{
            //    return Ok(result);
            ////}
            ///
            Employee employee = new Employee
            {
                Name = dto.Name,
                Age = dto.Age,
                Salary = dto.Salary
            };

            Employee result = await _employeeRepository.AddEmployee(employee,cancellationToken);

            EmployeeDto response = new EmployeeDto
            {
                Id = result.Id,
                Name = result.Name,
                Age = result.Age,
                Salary = result.Salary
            };

            //return Ok(response); //successful POST operation should return 201 Created, so we use CreatedAtAction which will route to GetEmpById and return newly created ID
            return CreatedAtAction(
                nameof(GetEmployeeById),
                new { id = response.Id },
            response);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateEmployee(int Id, UpdateEmployeeDto dto, CancellationToken cancellationToken) {
            Employee employee = new Employee
            {
                Id = Id,
                Name = dto.Name,
                Age = dto.Age,
                Salary = dto.Salary
            };

            //if (Id != employee.Id) // this check is invalid if we are using DTOs, now they are impossilbe to differ. Also UpdateDto don't contain Id, so client can't send multiple Ids (Only in URL not in Request body)
            //{
            //    return BadRequest();
            //}

            bool IsUpdateComplete = await _employeeRepository.UpdateEmployee(employee, cancellationToken);

            if (!IsUpdateComplete) 
            { 
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteEmployee(int Id, CancellationToken cancellationToken) { 
            bool isDeleted = await _employeeRepository.DeleteEmployee(Id, cancellationToken);

            if (isDeleted)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
