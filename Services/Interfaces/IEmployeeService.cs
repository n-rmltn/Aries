using Aries.Models;
using Aries.Models.ViewModels;


namespace Aries.Services.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeViewModel>> GetAllAsync();
    Task<EmployeeFormViewModel?> GetForEditAsync(int id);
    Task<bool> CreateAsync(EmployeeFormViewModel model);
    Task<bool> UpdateAsync(EmployeeFormViewModel model);
    Task<bool> DeleteAsync(int id);
    Task<bool> BulkDeleteAsync(IEnumerable<int> ids);
    Task<bool> BulkEditAsync(IEnumerable<int> ids, int departmentId);
    Task<DataTablesResponse<EmployeeViewModel>> GetPagedAsync(DataTablesRequest request);

}