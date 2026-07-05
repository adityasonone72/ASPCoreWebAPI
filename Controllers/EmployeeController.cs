using ASPCoreWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace ASPCoreWebAPI.Controllers
{
    [Route("api/Employee")]
    [ApiController]
    public class StudentController : Controller
    {

        public readonly IConfiguration _configuration;
        public StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet("GetAllEmployees")]
        //[Route("GetAllEmployees")]
        //[Obsolete]
        public ActionResult<List<Employee>> GetEmployees()
        {
            SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("EmployeeCon").ToString());
            SqlDataAdapter sqlData = new SqlDataAdapter("Select * FROM EmployeeInfo",sqlConnection);
            DataTable dt = new DataTable();
            sqlData.Fill(dt);


            List<Employee> empList = new List<Employee>();
            Response response = new Response();

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
                return Ok(empList);
            }
            else
            {
                return NotFound("No data found!!");
            }
            //return View();
        }
    }
}
