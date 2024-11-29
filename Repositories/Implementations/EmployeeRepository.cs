using Microsoft.EntityFrameworkCore;
using Aries.Data;
using Aries.Models;
using MySqlConnector;
using System.Data;

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

    // Lawd forgive the spaghetti
    public async Task<(IEnumerable<Employee> Data, int TotalRecords, int FilteredRecords)> 
    GetPagedAsync(int start, int length, string searchTerm, string orderName, string orderDir)
    {
        var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = "SP_Get_Employees_Paged";
        command.CommandType = CommandType.StoredProcedure;
        
        command.Parameters.Add(new MySqlParameter("p_Start", start));
        command.Parameters.Add(new MySqlParameter("p_Length", length));
        command.Parameters.Add(new MySqlParameter("p_Search", searchTerm ?? string.Empty));
        command.Parameters.Add(new MySqlParameter("p_OrderColumn", orderName));
        command.Parameters.Add(new MySqlParameter("p_OrderDir", orderDir));
        
        var result = new List<Employee>();
        int totalRecords = 0;
        int filteredRecords = 0;
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new Employee
            {
                Id = reader.GetInt32("Id"),
                Name = reader.GetString("Name"),
                DepartmentId = reader.GetInt32("DepartmentId"),
                Department = new Department
                {
                    Id = reader.GetInt32("DepartmentId"),
                    Name = reader.GetString("DepartmentName")
                }
            });
            if (result.Count == 1)
            {
                totalRecords = reader.GetInt32("TotalRecords");
                filteredRecords = reader.GetInt32("FilteredRecords");
            }
        }
        return (result, totalRecords, filteredRecords);
    }
}