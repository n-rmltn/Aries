using Aries.Models;
using Aries.Repositories.Interfaces;
using Aries.Services.Interfaces;

namespace Aries.Services.Implementations;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repository;
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(
        IDepartmentRepository repository,
        ILogger<DepartmentService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<DepartmentViewModel>> GetAllAsync()
    {
        try
        {
            return await _repository.GetDepartmentsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving departments");
            throw;
        }
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        try
        {
            return await _repository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving department {Id}", id);
            throw;
        }
    }

    public async Task<bool> CreateAsync(Department department)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(department.Name))
                return false;

            await _repository.CreateAsync(department);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating department");
            return false;
        }
    }

    public async Task<bool> UpdateAsync(Department department)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(department.Name))
                return false;

            var exists = await ExistsAsync(department.Id);
            if (!exists) return false;

            await _repository.UpdateAsync(department);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating department {Id}", department.Id);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            if (await HasEmployeesAsync(id))
                return false;

            await _repository.DeleteAsync(id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting department {Id}", id);
            return false;
        }
    }

    public async Task<bool> BulkDeleteAsync(IEnumerable<int> ids)
    {
        try
        {
            var validIds = new List<int>();
            foreach (var id in ids)
            {
                if (!await HasEmployeesAsync(id))
                {
                    validIds.Add(id);
                }
            }

            if (validIds.Any())
            {
                await _repository.BulkDeleteAsync(validIds);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk deleting departments");
            return false;
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var department = await GetByIdAsync(id);
        return department != null;
    }

    public async Task<bool> HasEmployeesAsync(int id)
    {
        var department = await GetByIdAsync(id);
        return department?.Employees.Any() ?? false;
    }
}