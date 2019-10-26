CREATE PROC usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT)
AS
	DECLARE @EmployeeDepartment INT = (
			SELECT e.DepartmentId
			FROM Employees e
			WHERE e.Id = @EmployeeId)

	DECLARE @CategoryDepartment INT = (
			SELECT c.DepartmentId
			FROM Categories c
			JOIN Reports r
			ON c.Id = r.CategoryId
			WHERE r.Id = @ReportId
	)

	IF(@EmployeeDepartment != @CategoryDepartment)
	BEGIN
		RAISERROR ('Employee doesn''t belong to the appropriate department!', 16, 1);
	END

	UPDATE Reports
		SET EmployeeId = @EmployeeId
		WHERE Id = @ReportId