using ASPCoreWebAPI.Models;

namespace ASPCoreWebAPI.Repository
{
    public interface IEmployeeRepository
    {
        List<Employee> GetEmployees();
    }
}
