CREATE PROCEDURE usp_GetEmployeesSalaryAbove35000
AS
	SELECT e.FirstName,
		   e.LastName
	FROM Employees e
	WHERE e.Salary > 35000