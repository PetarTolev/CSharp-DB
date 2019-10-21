SELECT f.Id,
	   f.[Name],
	   CONCAT(f.Size, 'KB')
FROM Files f
FULL JOIN Files fi
ON fi.ParentId = f.Id 
WHERE fi.Id IS NULL
ORDER BY f.Id,
		 f.[Name],
		 f.Size DESC