SELECT t.[Type], MAX(t.UsedLuggageTimes) AS 'MostUsedLuggage'
FROM 
(
SELECT lt.[Type],
	   ROW_NUMBER() OVER (PARTITION BY lt.[Type] ORDER BY p.Id ASC) AS 'UsedLuggageTimes'
FROM Luggages l
JOIN Passengers p
ON l.PassengerId = p.Id
JOIN LuggageTypes lt
ON l.LuggageTypeId = lt.Id
) AS t
GROUP BY t.[Type]
ORDER BY MostUsedLuggage DESC,
		 t.[Type]