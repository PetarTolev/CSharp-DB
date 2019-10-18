CREATE TRIGGER tr_DeleteEmployee
ON Employees
AFTER DELETE
AS
	INSERT INTO Deleted_Employees
	SELECT FirstName, LastName, MiddleName, JobTitle, DepartmentID, Salary
	FROM deleted
