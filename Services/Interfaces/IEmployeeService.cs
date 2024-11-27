namespace Aries.Services.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeViewModel>> GetAllAsync();
    Task<EmployeeFormViewModel?> GetForEditAsync(int id);
    Task<bool> CreateAsync(EmployeeFormViewModel model);
    Task<bool> UpdateAsync(EmployeeFormViewModel model);
    Task<bool> DeleteAsync(int id);
}