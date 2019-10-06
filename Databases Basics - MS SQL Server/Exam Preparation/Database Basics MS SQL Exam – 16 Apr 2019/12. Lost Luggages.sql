SELECT p.PassportId,
	   p.[Address]
FROM Passengers p
LEFT JOIN Luggages l
ON p.Id = l.PassengerId
WHERE l.PassengerId IS NULL
ORDER BY p.PassportId,
		 p.[Address]