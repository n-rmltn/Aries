using Microsoft.AspNetCore.Mvc.Rendering;

using Aries.Models;
using Aries.Repositories;
using Aries.Services.Interfaces;

namespace Aries.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        IEmployeeRepository repository,
        IDepartmentRepository departmentRepository,
        ILogger<EmployeeService> logger)
    {
        _repository = repository;
        _departmentRepository = departmentRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<EmployeeViewModel>> GetAllAsync()
    {
        try
        {
            return await _repository.GetEmployeesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees");
            throw;
        }
    }

    public async Task<EmployeeFormViewModel?> GetForEditAsync(int id)
    {
        try
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null) return null;

            var departments = await _departmentRepository.GetDepartmentsAsync();
            
            return new EmployeeFormViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                DepartmentId = employee.DepartmentId,
                Departments = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee {Id}", id);
            throw;
        }
    }

    public async Task<bool> CreateAsync(EmployeeFormViewModel model)
    {
        try
        {
            var employee = new Employee
            {
                Name = model.Name,
                DepartmentId = model.DepartmentId
            };

            await _repository.CreateAsync(employee);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee");
            return false;
        }
    }

    public async Task<bool> UpdateAsync(EmployeeFormViewModel model)
    {
        try
        {
            var employee = await _repository.GetByIdAsync(model.Id);
            if (employee == null) return false;

            employee.Name = model.Name;
            employee.DepartmentId = model.DepartmentId;

            await _repository.UpdateAsync(employee);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee {Id}", model.Id);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _repository.DeleteAsync(id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee {Id}", id);
            return false;
        }
    }

    public async Task<bool> BulkDeleteAsync(IEnumerable<int> ids)
    {
        try
        {
            await _repository.BulkDeleteAsync(ids);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk deleting employees");
            return false;
        }
    }

    public async Task<bool> BulkEditAsync(IEnumerable<int> ids, int departmentId)
    {
        try
        {
            await _repository.BulkEditAsync(ids, departmentId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk editing employees");
            return false;
        }
    }

    public async Task<DataTablesResponse<EmployeeViewModel>> GetPagedAsync(DataTablesRequest request)
    {
        var (data, totalRecords) = await _repository.GetPagedAsync(
            request.Start,
            request.Length);

        return new DataTablesResponse<EmployeeViewModel>
        {
            Draw = request.Draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = data.Select(e => new EmployeeViewModel
            {
                Id = e.Id,
                Name = e.Name,
                DepartmentName = e.Department?.Name ?? "No Department"
            })
        };
    }
}