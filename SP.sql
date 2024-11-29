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

-- SP Bulk Remove Department
DROP PROCEDURE IF EXISTS SP_Bulk_Remove_Department;
CREATE PROCEDURE SP_Bulk_Remove_Department(IN p_Ids VARCHAR(1000))
BEGIN
    -- Delete departments that have no employees
    DELETE d FROM Departments d
    LEFT JOIN Employees e ON d.Id = e.DepartmentId
    WHERE FIND_IN_SET(d.Id, p_Ids) > 0
    AND e.Id IS NULL;
END

-- SP Get Employees
DROP PROCEDURE IF EXISTS SP_Get_Employees;
CREATE PROCEDURE SP_Get_Employees()
BEGIN
    SELECT e.Id, e.Name, e.DepartmentId, d.Name as DepartmentName
    FROM Employees e
    INNER JOIN Departments d ON e.DepartmentId = d.Id;
END

-- SP Insert Employee
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

-- SP Remove Employee
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

-- SP Remove Employee
DROP PROCEDURE IF EXISTS SP_Remove_Employee;
CREATE PROCEDURE SP_Remove_Employee(IN p_Id INT)
BEGIN
    DELETE FROM Employees 
    WHERE Id = p_Id;
END

-- SP Bulk Remove Employee
DROP PROCEDURE IF EXISTS SP_Bulk_Remove_Employee;
CREATE PROCEDURE SP_Bulk_Remove_Employee(IN p_Ids VARCHAR(1000))
BEGIN
    DELETE FROM Employees 
    WHERE FIND_IN_SET(Id, p_Ids) > 0;
END

-- SP Bulk Edit Employee
DROP PROCEDURE IF EXISTS SP_Bulk_Edit_Employee;
CREATE PROCEDURE SP_Bulk_Edit_Employee(IN p_Ids VARCHAR(1000), IN p_DepartmentId INT)
BEGIN
    UPDATE Employees 
    SET DepartmentId = p_DepartmentId
    WHERE FIND_IN_SET(Id, p_Ids) > 0;
END

-- Lord help me
DROP PROCEDURE IF EXISTS SP_Get_Employees_Paged;
CREATE PROCEDURE SP_Get_Employees_Paged(
    IN p_Start INT,
    IN p_Length INT
)
BEGIN
    DECLARE total_records INT;

    -- record num
    SELECT COUNT(*) INTO total_records
    FROM Employees e
    INNER JOIN Departments d ON e.DepartmentId = d.Id;

    SELECT 
        e.Id,
        e.Name,
        e.DepartmentId,
        d.Name as DepartmentName,
        total_records as TotalRecords
    FROM Employees e
    INNER JOIN Departments d ON e.DepartmentId = d.Id
    ORDER BY e.Name ASC
    LIMIT p_Start, p_Length;
END;
