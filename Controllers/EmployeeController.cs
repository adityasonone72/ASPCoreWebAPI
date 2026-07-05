using ASPCoreWebAPI.Models;
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

        public readonly IConfiguration _configuration;
        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet("GetAllEmployees")]
        //[Route("GetAllEmployees")]
        //[Obsolete]
        public ActionResult<List<Employee>> GetEmployees()
        {
            using SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("EmployeeCon").ToString()); //using keyword gaurantees that Dispose() will be called even if sql connection failed!! So, the database can be accissible for others also.
            using SqlDataAdapter sqlData = new SqlDataAdapter("Select * FROM EmployeeInfo",sqlConnection);
            DataTable dt = new DataTable();
            sqlData.Fill(dt);


            List<Employee> empList = new List<Employee>();
            //Response response = new Response(); //using this for editing response messages, but now we are using NotFound()

            if (dt.Rows.Count > 0)
            {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    Employee employee = new Employee();
                    employee.Name = Convert.ToString(dt.Rows[i]["Name"]);
                    employee.Age = Convert.ToInt32(dt.Rows[i]["Age"]);
                    employee.Salary = Convert.ToDouble(dt.Rows[i]["Salary"]);

                    empList.Add(employee);
                }
            }
            if (empList.Count > 0)
            {
                return Ok(empList); //creates object of OkObjectResult, which will give 200Ok statusCode, and uses System.Text.Json for serialization
            }
            else
            {
                return NotFound("No data found!!");
            }

            //return View();
        }
    }
}
