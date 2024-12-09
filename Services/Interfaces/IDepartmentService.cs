using Aries.Models.ViewModels;
using Aries.Models.Entities;

namespace Aries.Services.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentViewModel>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Department department);
    Task<bool> UpdateAsync(Department department);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> HasEmployeesAsync(int id);
    Task<bool> BulkDeleteAsync(IEnumerable<int> ids);
}