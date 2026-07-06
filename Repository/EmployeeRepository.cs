using ASPCoreWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ASPCoreWebAPI.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public readonly IConfiguration _configuration;
        public EmployeeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        //[Route("GetAllEmployees")]
        //[Obsolete]
        public List<Employee> GetEmployees()
        {
            using SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("EmployeeCon").ToString()); //using keyword gaurantees that Dispose() will be called even if sql connection failed!! So, the database can be accissible for others also.
            using SqlDataAdapter sqlData = new SqlDataAdapter("Select * FROM EmployeeInfo", sqlConnection);
            DataTable dt = new DataTable();
            sqlData.Fill(dt);


            List<Employee> empList = new List<Employee>();
            //Response response = new Response(); //using this for editing response messages, but now we are using NotFound()

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Employee employee = new Employee();
                    employee.Name = Convert.ToString(dt.Rows[i]["Name"]);
                    employee.Age = Convert.ToInt32(dt.Rows[i]["Age"]);
                    employee.Salary = Convert.ToDouble(dt.Rows[i]["Salary"]);
                    employee.Id = Convert.ToInt32(dt.Rows[i]["Id"]);

                    empList.Add(employee);
                }
            }

            return empList;
        }
        [HttpGet("{Id}")]
        public Employee? GetEmployeeById(int id) {
            using SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("EmployeeCon").ToString());
            using SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("Select * FROM EmployeeInfo where Id = @Id",sqlConnection);

            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Id", id);
            DataTable dt = new DataTable();

            sqlDataAdapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                Employee emp = new Employee();
                DataRow row = dt.Rows[0];
                emp.Name = Convert.ToString(row["Name"]);
                emp.Age = Convert.ToInt32(row["Age"]);
                emp.Salary = Convert.ToDouble(row["Salary"]);
                emp.Id = Convert.ToInt32(row["Id"]);
                return emp;
            }
            return null;
        }
    }
}
