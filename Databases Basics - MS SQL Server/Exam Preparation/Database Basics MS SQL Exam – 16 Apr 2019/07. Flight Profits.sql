SELECT t.FlightId,
	   SUM(t.Price) AS 'Price'
FROM Tickets t
GROUP BY FlightId
ORDER BY Price DESC, 
		 FlightId