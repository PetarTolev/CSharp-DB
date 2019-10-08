CREATE OR ALTER PROC usp_GetTownsStartingWith (@String VARCHAR(10))
AS
	SELECT t.[Name]
	FROM Towns t
	WHERE LEFT(t.[Name], LEN(@String)) = @String
