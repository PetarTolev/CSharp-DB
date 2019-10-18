CREATE PROC usp_AssignProject(@EmployeeId INT, @ProjectId INT)
AS
BEGIN TRANSACTION
	INSERT INTO EmployeesProjects 
	VALUES (@employeeId, @projectID)

	DECLARE @projectsCount INT = 
	(	SELECT COUNT(*)
		FROM EmployeesProjects ep
		WHERE ep.EmployeeID = @EmployeeId)

	IF(@projectsCount > 3)
	BEGIN
		ROLLBACK
		RAISERROR('The employee has too many projects!', 16, 1)
	END
COMMIT