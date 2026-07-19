using ASPCoreWebAPI.Models;

namespace ASPCoreWebAPI.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetEmployees(); //GET -> All Emp details

        Task<Employee?> GetEmployeeById(int Id); // GET -> By Id

        Task<Employee> AddEmployee(Employee employee); //POST -> this will return latest generated emp id

        Task<bool> UpdateEmployee(Employee employee); //PUT -> 204 No content (either updation will happen or won't)

        Task<bool> DeleteEmployee(int Id); //DELETE -> 204 No content (either delition will happen or won't)
    }
}
