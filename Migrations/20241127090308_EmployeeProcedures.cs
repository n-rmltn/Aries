using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aries.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS SP_Get_Employees;
                CREATE PROCEDURE SP_Get_Employees()
                BEGIN
                    SELECT e.Id, e.Name, e.DepartmentId, d.Name as DepartmentName
                    FROM Employees e
                    INNER JOIN Departments d ON e.DepartmentId = d.Id;
                END
            ");

            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS SP_Insert_Employee;
                CREATE PROCEDURE SP_Insert_Employee(
                    IN p_Name VARCHAR(100),
                    IN p_DepartmentId INT
                )
                BEGIN
                    INSERT INTO Employees (Name, DepartmentId) 
                    VALUES (p_Name, p_DepartmentId);
                    SELECT LAST_INSERT_ID() as Id;
                END
            ");

            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS SP_Edit_Employee;
                CREATE PROCEDURE SP_Edit_Employee(
                    IN p_Id INT,
                    IN p_Name VARCHAR(100),
                    IN p_DepartmentId INT
                )
                BEGIN
                    UPDATE Employees 
                    SET Name = p_Name,
                        DepartmentId = p_DepartmentId
                    WHERE Id = p_Id;
                END
            ");

            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS SP_Remove_Employee;
                CREATE PROCEDURE SP_Remove_Employee(IN p_Id INT)
                BEGIN
                    DELETE FROM Employees 
                    WHERE Id = p_Id;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS SP_Get_Employees;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS SP_Insert_Employee;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS SP_Edit_Employee;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS SP_Remove_Employee;");
        }
    }
}
