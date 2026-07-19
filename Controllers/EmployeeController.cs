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
        public async Task<ActionResult<List<Employee>>> GetEmployees()
        {
            //Added Dependency Injection,  Now controller don't need to handle SQL connection, running queries, etc. 
            //EmployeeController now only has one job, which is to handle http request and response.

            //Task<List<Employee>> employees = _employeeRepository.GetEmployees();

            List<Employee> result = await _employeeRepository.GetEmployees();
            return Ok(result); //creates object of OkObjectResult, which will give 200Ok statusCode, and uses System.Text.Json for 
            //return View();
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<Employee?>> GetEmployeeById(int Id)
        {
            var result = await _employeeRepository.GetEmployeeById(Id);

            if (result == null) {
                return NotFound("Employee Record Not found for this Id");
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            var result = await _employeeRepository.AddEmployee(employee);

            //if (result == null) { //it will never be  null, even if insertion failed.
            //    return BadRequest("Insertion falied");
            //}
            //else
            //{
                return Ok(result);
            //}
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateEmployee(int Id, Employee employee) { 
            if(Id != employee.Id)
            {
                return BadRequest();
            }

            bool IsUpdateComplete = await _employeeRepository.UpdateEmployee(employee);

            if (!IsUpdateComplete) 
            { 
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteEmployee(int Id) { 
            bool isDeleted = await _employeeRepository.DeleteEmployee(Id);

            if (isDeleted)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
