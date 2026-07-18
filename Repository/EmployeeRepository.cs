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
                    employee.Salary = Convert.ToDecimal(dt.Rows[i]["Salary"]);
                    employee.Id = Convert.ToInt32(dt.Rows[i]["Id"]);

                    empList.Add(employee);
                }
            }

            return empList;
        }
        public Employee? GetEmployeeById(int id) {
            using SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("EmployeeCon").ToString());
            sqlConnection.Open();
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
                emp.Salary = Convert.ToDecimal(row["Salary"]);
                emp.Id = Convert.ToInt32(row["Id"]);
                return emp;
            }
            return null;
        }

        public Employee AddEmployee(Employee emp) {
            // using keyword dispose the connection if any exception is occured while CRUD.
            using SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("EmployeeCon").ToString());
            using SqlCommand sqlCommand = new SqlCommand("Insert into EmployeeInfo(Name,Age,Salary) VALUES(@Name,@Age,@Salary); SELECT SCOPE_IDENTITY();", sqlConnection);

            sqlConnection.Open();

            sqlCommand.Parameters.AddWithValue("@Name", emp.Name);
            //sqlCommand.Parameters.AddWithValue("@Age", emp.Age); //AddWithValue guesses datatype automatically. Might fail in some cases, that's why using Add is a best practice.
            sqlCommand.Parameters.Add("@Age", SqlDbType.Int).Value = emp.Age;
            sqlCommand.Parameters.AddWithValue("@Salary", emp.Salary);

            try
            {
                int generatedId = Convert.ToInt32(sqlCommand.ExecuteScalar());
                emp.Id = generatedId;
                return emp;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public bool UpdateEmployee(Employee emp) {
            using SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("EmployeeCon").ToString());
            using SqlCommand sqlCommand = new SqlCommand("UPDATE EmployeeInfo SET Name = @Name, Age = @Age, Salary = @Salary WHERE Id = @Id", sqlConnection); 

            sqlCommand.Parameters.Add("@Name", SqlDbType.VarChar).Value = emp.Name;
            sqlCommand.Parameters.Add("@Age", SqlDbType.Int).Value = emp.Age;
            sqlCommand.Parameters.Add("@Salary", SqlDbType.Decimal).Value = emp.Salary;
            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = emp.Id;

            sqlConnection.Open();

            int rowAffected = sqlCommand.ExecuteNonQuery();

            return rowAffected == 1;
        }

        public bool DeleteEmployee(int Id) {
            using SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("EmployeeCon").ToString());
            using SqlCommand sqlCommand = new SqlCommand("DELETE FROM EmployeeInfo WHERE Id = @Id", sqlConnection);

            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = Id;

            sqlConnection.Open();

            int rowAffected = sqlCommand.ExecuteNonQuery();

            return rowAffected == 1;
        }
    }
}
