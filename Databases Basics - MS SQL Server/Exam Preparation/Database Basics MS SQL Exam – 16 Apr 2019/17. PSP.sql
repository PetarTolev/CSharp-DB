SELECT pl.[Name],
	   pl.Seats,
	   COUNT(p.Id) AS 'Passenger Count'
FROM Tickets t
JOIN Passengers p
ON t.PassengerId = p.Id
FULL JOIN Flights f
ON t.FlightId = f.Id
FULL JOIN Planes pl
ON f.PlaneId = pl.Id
GROUP BY pl.[Name], pl.Seats
ORDER BY [Passenger Count] DESC,
		 pl.[Name],
		 pl.Seats