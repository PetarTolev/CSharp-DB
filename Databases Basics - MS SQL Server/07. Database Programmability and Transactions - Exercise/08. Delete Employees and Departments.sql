CREATE PROC usp_DeleteEmployeesFromDepartment (@departmentId INT)
AS
BEGIN
	ALTER TABLE Departments
	ALTER COLUMN ManagerID INT NULL

	DELETE FROM EmployeesProjects
	WHERE EmployeeID IN (SELECT EmployeeId FROM Employees WHERE DepartmentID = @departmentId)

	UPDATE Departments
	SET ManagerID = NULL
	WHERE DepartmentID = @departmentId

	UPDATE Employees
	SET ManagerID = NULL
	WHERE ManagerID IN (SELECT EmployeeId FROM Employees WHERE DepartmentID = @departmentId)

	DELETE FROM Employees
	WHERE DepartmentID = @departmentId

	DELETE FROM Departments 
	WHERE DepartmentID = @departmentId

	SELECT COUNT(*)
	FROM Employees e
	WHERE e.DepartmentID = @departmentId
END