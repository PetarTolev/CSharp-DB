SELECT p.FirstName, p.LastName, COUNT(t.Id) AS 'Total Trips'
FROM Tickets t
FULL JOIN Passengers p
ON t.PassengerId = p.Id
GROUP BY p.FirstName, p.LastName
ORDER BY [Total Trips] DESC,
		 p.FirstName,
		 p.LastName