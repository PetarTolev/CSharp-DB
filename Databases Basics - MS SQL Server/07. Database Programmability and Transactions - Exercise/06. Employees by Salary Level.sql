CREATE PROC usp_EmployeesBySalaryLevel @SalaryLevel NVARCHAR(10)
AS
BEGIN
	IF(@SalaryLevel = 'Low')
	BEGIN
		SELECT e.FirstName,
			   e.LastName
		FROM Employees e
		WHERE e.Salary < 30000
	END
	ELSE IF(@SalaryLevel = 'Average')
	BEGIN
		SELECT e.FirstName,
			   e.LastName
		FROM Employees e
		WHERE e.Salary >= 30000 AND e.Salary <= 50000
	END
	ELSE IF(@SalaryLevel = 'High')
	BEGIN
		SELECT e.FirstName,
			   e.LastName
		FROM Employees e
		WHERE e.Salary > 50000
	END
END
