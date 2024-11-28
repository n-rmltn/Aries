using Microsoft.EntityFrameworkCore;
using Aries.Data;
using Aries.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;

namespace Aries.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DepartmentViewModel>> GetDepartmentsAsync()
    {
        var results = await _context.Database
        .SqlQuery<DepartmentSpResult>($"CALL SP_Get_Departments")
        .ToListAsync();

        return results.Select(r => new DepartmentViewModel
        {
            Id = r.Id,
            Name = r.Name,
            EmployeeCount = r.EmployeeCount
        });
    }

    public async Task<Department> GetByIdAsync(int id)
    {
        return await _context.Departments
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task CreateAsync(Department department)
    {
        var nameParam = new MySqlParameter("@p_Name", department.Name);
        var result = await _context.Database
            .ExecuteSqlRawAsync("CALL SP_Insert_Department(@p_Name)", nameParam);
        
        var newId = await _context.Departments
            .OrderByDescending(d => d.Id)
            .Select(d => d.Id)
            .FirstOrDefaultAsync();
            
        department.Id = newId;
    }

    public async Task UpdateAsync(Department department)
    {
        var parameters = new[]
        {
            new MySqlParameter("@p_Id", department.Id),
            new MySqlParameter("@p_Name", department.Name)
        };

        await _context.Database
            .ExecuteSqlRawAsync("CALL SP_Edit_Department(@p_Id, @p_Name)", parameters);
    }

    public async Task DeleteAsync(int id)
    {
        var parameter = new MySqlParameter("@p_Id", id);
        await _context.Database
            .ExecuteSqlRawAsync("CALL SP_Remove_Department(@p_Id)", parameter);
    }

    public async Task BulkDeleteAsync(IEnumerable<int> ids)
    {
        var idsString = string.Join(",", ids);
        var parameter = new MySqlParameter("@p_Ids", idsString);
        await _context.Database
            .ExecuteSqlRawAsync("CALL SP_Bulk_Remove_Department(@p_Ids)", parameter);
    }
}