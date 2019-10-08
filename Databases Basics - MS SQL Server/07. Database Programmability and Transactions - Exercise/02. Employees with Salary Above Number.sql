CREATE PROCEDURE usp_GetEmployeesSalaryAboveNumber @Number DECIMAL(18, 4)
AS
	SELECT e.FirstName,
		   e.LastName
	FROM Employees e
	WHERE e.Salary >= @Number
