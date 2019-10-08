CREATE FUNCTION ufn_GetSalaryLevel (@Salary DECIMAL(18,4))
RETURNS NVARCHAR(10)
AS
BEGIN
	DECLARE @result  NVARCHAR(10)

	IF(@Salary < 30000)
	BEGIN
		SET @result = 'Low'
	END
	ELSE IF(@Salary >= 30000 AND @Salary <= 50000)
	BEGIN
		SET @result = 'Average'
	END
	ELSE
	BEGIN
		SET @result = 'High'
	END

	RETURN @result
END

SELECT e.Salary,
	   dbo.ufn_GetSalaryLevel(e.Salary) AS 'Salary Level'
FROM Employees e