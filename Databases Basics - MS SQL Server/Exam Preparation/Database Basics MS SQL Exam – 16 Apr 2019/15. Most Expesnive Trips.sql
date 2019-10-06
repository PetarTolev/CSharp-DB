SELECT k.FirstName,
	   k.LastName,
	   k.Destination,
	   k.Price
FROM
(
SELECT p.FirstName, 
	   p.LastName,
	   f.Destination,
	   t.Price,
	   DENSE_RANK() OVER (PARTITION BY p.FirstName ORDER BY t.Price DESC) AS 'Rank'
FROM Tickets t
JOIN Passengers p
ON t.PassengerId = p.Id
JOIN Flights f
ON t.FlightId = f.Id
) AS k
WHERE k.[Rank] = 1
ORDER BY k.Price DESC,
		 k.FirstName,
		 k.LastName,
		 k.Destination