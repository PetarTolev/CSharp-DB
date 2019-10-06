SELECT CONCAT(p.FirstName, ' ', p.LastName) AS 'Full Name',
	   pl.[Name] AS 'Plane Name',
	   CONCAT(f.Origin, ' - ', f.Destination) AS 'Trip',
	   lt.[Type] AS 'Luggage Type'
FROM Tickets t
JOIN Passengers p
ON t.PassengerId = p.Id
JOIN Flights f
ON t.FlightId = f.Id
JOIN Planes pl
ON f.PlaneId = pl.Id
JOIN Luggages l
ON t.LuggageId = l.Id
JOIN LuggageTypes lt
ON l.LuggageTypeId = lt.Id
ORDER BY [Full Name],
		 pl.[Name],
		 f.Origin,
		 f.Destination,
		 lt.[Type]

