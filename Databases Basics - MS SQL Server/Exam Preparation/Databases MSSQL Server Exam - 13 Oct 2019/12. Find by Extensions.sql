CREATE PROC usp_FindByExtension(@extension NVARCHAR(10))
AS
SELECT f.Id,
	   f.[Name],
	   CONCAT(f.Size, 'KB') AS 'Size'
FROM Files f
WHERE f.Name LIKE '%' + @extension

EXEC usp_FindByExtension 'txt'