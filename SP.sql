-- SP Get Departments
DROP PROCEDURE IF EXISTS SP_Get_Departments;
CREATE PROCEDURE SP_Get_Departments()
BEGIN
    SELECT d.Id, d.Name, COUNT(e.Id) as EmployeeCount
    FROM Departments d
    LEFT JOIN Employees e ON d.Id = e.DepartmentId
    GROUP BY d.Id, d.Name;
END

-- SP Get Employees
DROP PROCEDURE IF EXISTS SP_Insert_Department;
CREATE PROCEDURE SP_Insert_Department(IN p_Name VARCHAR(100))
BEGIN
    INSERT INTO Departments (Name) VALUES (p_Name);
    SELECT LAST_INSERT_ID() as Id;
END

-- SP Edit Department
DROP PROCEDURE IF EXISTS SP_Edit_Department;
CREATE PROCEDURE SP_Edit_Department(IN p_Id INT, IN p_Name VARCHAR(100))
BEGIN
    UPDATE Departments 
    SET Name = p_Name
    WHERE Id = p_Id;
END

-- SP Remove Department
DROP PROCEDURE IF EXISTS SP_Remove_Department;
CREATE PROCEDURE SP_Remove_Department(IN p_Id INT)
BEGIN
    DELETE FROM Departments 
    WHERE Id = p_Id;
END

-- SP Bulk Remove Employee
DROP PROCEDURE IF EXISTS SP_Bulk_Remove_Employee;
CREATE PROCEDURE SP_Bulk_Remove_Employee(IN p_Ids VARCHAR(1000))
BEGIN
    DELETE FROM Employees 
    WHERE FIND_IN_SET(Id, p_Ids) > 0;
END