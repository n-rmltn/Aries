using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aries.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentProcedure : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
            DROP PROCEDURE IF EXISTS SP_Get_Departments;
            CREATE PROCEDURE SP_Get_Departments()
            BEGIN
                SELECT d.Id, d.Name, COUNT(e.Id) as EmployeeCount
                FROM Departments d
                LEFT JOIN Employees e ON d.Id = e.DepartmentId
                GROUP BY d.Id, d.Name;
            END
        ");

        migrationBuilder.Sql(@"
            DROP PROCEDURE IF EXISTS SP_Insert_Department;
            CREATE PROCEDURE SP_Insert_Department(IN p_Name VARCHAR(100))
            BEGIN
                INSERT INTO Departments (Name) VALUES (p_Name);
                SELECT LAST_INSERT_ID() as Id;
            END
        ");

        migrationBuilder.Sql(@"
            DROP PROCEDURE IF EXISTS SP_Edit_Department;
            CREATE PROCEDURE SP_Edit_Department(IN p_Id INT, IN p_Name VARCHAR(100))
            BEGIN
                UPDATE Departments 
                SET Name = p_Name
                WHERE Id = p_Id;
            END
        ");

        migrationBuilder.Sql(@"
            DROP PROCEDURE IF EXISTS SP_Remove_Department;
            CREATE PROCEDURE SP_Remove_Department(IN p_Id INT)
            BEGIN
                DELETE FROM Departments 
                WHERE Id = p_Id;
            END
        ");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS SP_Get_Departments;");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS SP_Insert_Department;");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS SP_Edit_Department;");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS SP_Remove_Department;");
    }
}
}
