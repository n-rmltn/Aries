using Microsoft.EntityFrameworkCore;
using Aries.Data;
using Aries.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;

namespace Aries.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EmployeeViewModel>> GetEmployeesAsync()
    {
        return await _context.Database
            .SqlQuery<EmployeeViewModel>($"CALL SP_Get_Employees")
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task CreateAsync(Employee employee)
    {
        var parameters = new[]
        {
            new MySqlParameter("@p_Name", employee.Name),
            new MySqlParameter("@p_DepartmentId", employee.DepartmentId)
        };

        await _context.Database
            .ExecuteSqlRawAsync("CALL SP_Insert_Employee(@p_Name, @p_DepartmentId)", parameters);
    }

    public async Task UpdateAsync(Employee employee)
    {
        var parameters = new[]
        {
            new MySqlParameter("@p_Id", employee.Id),
            new MySqlParameter("@p_Name", employee.Name),
            new MySqlParameter("@p_DepartmentId", employee.DepartmentId)
        };

        await _context.Database
            .ExecuteSqlRawAsync("CALL SP_Edit_Employee(@p_Id, @p_Name, @p_DepartmentId)", parameters);
    }

    public async Task DeleteAsync(int id)
    {
        var parameter = new MySqlParameter("@p_Id", id);
        await _context.Database
            .ExecuteSqlRawAsync("CALL SP_Remove_Employee(@p_Id)", parameter);
    }

    public async Task BulkDeleteAsync(IEnumerable<int> ids)
    {
        var idsString = string.Join(",", ids);
        var parameter = new MySqlParameter("@p_Ids", idsString);
        await _context.Database
            .ExecuteSqlRawAsync("CALL SP_Bulk_Remove_Employee(@p_Ids)", parameter);
    }

    public async Task BulkEditAsync(IEnumerable<int> ids, int departmentId)
{
    var idsString = string.Join(",", ids);
    var parameters = new[]
    {
        new MySqlParameter("@p_Ids", idsString),
        new MySqlParameter("@p_DepartmentId", departmentId)
    };
    await _context.Database
        .ExecuteSqlRawAsync("CALL SP_Bulk_Edit_Employee(@p_Ids, @p_DepartmentId)", parameters);
}
}