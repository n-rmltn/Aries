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
    Task BulkDeleteAsync(IEnumerable<int> ids);
    Task BulkEditAsync(IEnumerable<int> ids, int departmentId);
    Task<(IEnumerable<Employee> Data, int TotalRecords, int FilteredRecords)> 
    GetPagedAsync(int start, int length, string searchTerm);
}