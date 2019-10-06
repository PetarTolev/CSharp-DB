SELECT f.Destination,
		COUNT(t.Id) AS 'TripsCount'
FROM Tickets t
FULL JOIN Flights f
ON t.FlightId = f.Id
GROUP BY Destination
ORDER BY TripsCount DESC,
		 f.Destination