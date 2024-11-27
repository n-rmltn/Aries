using System.Collections.Generic;
using System.Threading.Tasks;
using Aries.Models;

public interface IEmployeeRepository
{
    Task<IEnumerable<EmployeeViewModel>> GetEmployeesAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task CreateAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(int id);
}