using ASPCoreWebAPI.Models;
using ASPCoreWebAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; //it is no longer needed, as we are not using Json.Serialize, Ok() uses System.Text.Json for serialization
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace ASPCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

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
        [HttpGet("GetAllEmployees")]
        //[Route("GetAllEmployees")]
        //[Obsolete]
        public ActionResult<List<Employee>> GetEmployees()
        {
            //Added Dependency Injection,  Now controller don't need to handle SQL connection, running queries, etc. 
            //EmployeeController now only has one job, which is to handle http request and response.
            var employees = _employeeRepository.GetEmployees();

            if (employees.Count > 0)
            {
                return Ok(employees); //creates object of OkObjectResult, which will give 200Ok statusCode, and uses System.Text.Json for serialization
            }
            else
            {
                return NotFound("No data found!!");
            }

            //return View();
        }
    }
}
