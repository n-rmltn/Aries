using Aries.Models;
using Aries.Models.DTOs;

namespace Aries.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<EmployeeViewModel>> GetEmployeesAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task CreateAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(int id);
    Task BulkDeleteAsync(IEnumerable<int> ids);
    Task BulkEditAsync(IEnumerable<int> ids, int departmentId);
    Task<PagedResultDto<Employee>> GetPagedAsync(int start, int length, string searchTerm, string orderName, string orderDir);
}