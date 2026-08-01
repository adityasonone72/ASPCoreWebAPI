using ASPCoreWebAPI.Models;

namespace ASPCoreWebAPI.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetEmployees(CancellationToken cancellationToken); //GET -> All Emp details

        Task<Employee?> GetEmployeeById(int Id,CancellationToken cancellationToken); // GET -> By Id

        Task<Employee> AddEmployee(Employee employee, CancellationToken cancellationToken); //POST -> this will return latest generated emp id

        Task<bool> UpdateEmployee(Employee employee, CancellationToken cancellationToken); //PUT -> 204 No content (either updation will happen or won't)

        Task<bool> DeleteEmployee(int Id, CancellationToken cancellationToken); //DELETE -> 204 No content (either delition will happen or won't)
    }
}
