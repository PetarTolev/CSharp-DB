CREATE PROC usp_GetEmployeesFromTown @TownName NVARCHAR(14)
AS
BEGIN
	SELECT e.FirstName, e.LastName
	FROM Employees e
	JOIN Addresses a
	ON e.AddressID = a.AddressID
	JOIN Towns t
	ON t.TownID = a.TownID
	WHERE t.[Name] = @TownName
END