using Aries.Models;

namespace Aries.Repositories.Interfaces;

public interface IDepartmentRepository
{
    Task<IEnumerable<DepartmentViewModel>> GetDepartmentsAsync();
    Task<Department> GetByIdAsync(int id);
    Task CreateAsync(Department department);
    Task UpdateAsync(Department department);
    Task DeleteAsync(int id);
    Task BulkDeleteAsync(IEnumerable<int> ids);
}