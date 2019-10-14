SELECT c.Id,
	   CONCAT(c.FirstName, ' ', c.LastName),
	   c.Ucn
FROM Colonists c
ORDER BY c.FirstName,
		 c.LastName,
		 c.Id