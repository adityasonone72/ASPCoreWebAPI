using ASPCoreWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
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
        public async Task<List<Employee>> GetEmployees()
        {
            using SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("EmployeeCon").ToString()); //using keyword gaurantees that Dispose() will be called even if sql connection failed!! So, the database can be accissible for others also.
            using SqlCommand sqlData = new SqlCommand("Select * FROM EmployeeInfo", sqlConnection); //sqlDataAdapter don't provide us async methods
            
            //DataTable dt = new DataTable();
            //sqlData.Fill(dt);

           await sqlConnection.OpenAsync();

            List<Employee> empList = new List<Employee>();

            using SqlDataReader reader = await sqlData.ExecuteReaderAsync();

            while(await reader.ReadAsync())
            {
                Employee emp = new Employee();
                emp.Id = Convert.ToInt32(reader["Id"]);
                emp.Name = Convert.ToString(reader["Name"]);
                emp.Age = Convert.ToInt32(reader["Age"]);
                emp.Salary = Convert.ToDecimal(reader["Salary"]);

                empList.Add(emp);
            }
            return empList;
        }
        public async Task<Employee?> GetEmployeeById(int id) {
            using SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("EmployeeCon").ToString());
            
            await sqlConnection.OpenAsync();
            using SqlCommand sqlCommand = new SqlCommand("Select * FROM EmployeeInfo where Id = @Id",sqlConnection);

            sqlCommand.Parameters.AddWithValue("@Id", id);
            using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
            

            if(await reader.ReadAsync()) //instead of while, we have used if here. In while we know that we might get 0 or 100 rows, but here since we are finding by primary key, we either get 0 or 1.
            {
                Employee emp = new Employee();
                emp.Id = Convert.ToInt32(reader["Id"]);
                emp.Name = Convert.ToString(reader["Name"]);
                emp.Age = Convert.ToInt32(reader["Age"]);
                emp.Salary = Convert.ToDecimal(reader["Salary"]);
                return emp;
            }
            return null;
        }

        public async Task<Employee> AddEmployee(Employee emp) {
            // using keyword dispose the connection if any exception is occured while CRUD.
            using SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("EmployeeCon").ToString());
            using SqlCommand sqlCommand = new SqlCommand("Insert into EmployeeInfo(Name,Age,Salary) VALUES(@Name,@Age,@Salary); SELECT SCOPE_IDENTITY();", sqlConnection);

            await sqlConnection.OpenAsync();

            sqlCommand.Parameters.AddWithValue("@Name", emp.Name);
            //sqlCommand.Parameters.AddWithValue("@Age", emp.Age); //AddWithValue guesses datatype automatically. Might fail in some cases, that's why using Add is a best practice.
            sqlCommand.Parameters.Add("@Age", SqlDbType.Int).Value = emp.Age;
            sqlCommand.Parameters.AddWithValue("@Salary", emp.Salary);

            int generatedId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());
            emp.Id = generatedId;
            return emp;
        }

        public async Task<bool> UpdateEmployee(Employee emp) {
            using SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("EmployeeCon").ToString());
            using SqlCommand sqlCommand = new SqlCommand("UPDATE EmployeeInfo SET Name = @Name, Age = @Age, Salary = @Salary WHERE Id = @Id", sqlConnection); 

            sqlCommand.Parameters.Add("@Name", SqlDbType.VarChar).Value = emp.Name;
            sqlCommand.Parameters.Add("@Age", SqlDbType.Int).Value = emp.Age;
            sqlCommand.Parameters.Add("@Salary", SqlDbType.Decimal).Value = emp.Salary;
            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = emp.Id;

            await sqlConnection.OpenAsync();

            int rowAffected = await sqlCommand.ExecuteNonQueryAsync();

            return rowAffected == 1;
        }

        public async Task<bool> DeleteEmployee(int Id) {
            using SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("EmployeeCon").ToString());
            using SqlCommand sqlCommand = new SqlCommand("DELETE FROM EmployeeInfo WHERE Id = @Id", sqlConnection);

            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = Id;

            await sqlConnection.OpenAsync();

            int rowAffected = await sqlCommand.ExecuteNonQueryAsync();

            return rowAffected == 1;
        }
    }
}
