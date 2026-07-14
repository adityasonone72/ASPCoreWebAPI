using ASPCoreWebAPI.Models;

namespace ASPCoreWebAPI.Repository
{
    public interface IEmployeeRepository
    {
        List<Employee> GetEmployees(); //GET -> All Emp details

        Employee? GetEmployeeById(int Id); // GET -> By Id

        Employee AddEmployee(Employee employee); //POST -> this will return latest generated emp id
    }
}
