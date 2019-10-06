SELECT p.Id,
	   p.[Name],
	   p.Seats,
	   p.[Range]
FROM Planes p
WHERE p.[Name] LIKE '%tr%'
ORDER BY p.Id,
		 p.[Name],
		 p.Seats,
		 p.[Range]
