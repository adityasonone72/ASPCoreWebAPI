using ASPCoreWebAPI.Models;

namespace ASPCoreWebAPI.Repository
{
    public interface IEmployeeRepository
    {
        List<Employee> GetEmployees(); //GET -> All Emp details

        Employee? GetEmployeeById(int Id); // GET -> By Id

        Employee AddEmployee(Employee employee); //POST -> this will return latest generated emp id

        bool UpdateEmployee(Employee employee); //PUT -> 204 No content (either updation will happen or won't)

        bool DeleteEmployee(int Id); //DELETE -> 204 No content (either delition will happen or won't)
    }
}
