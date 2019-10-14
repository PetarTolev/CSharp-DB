SELECT p.[Name] AS 'PlanetName',
	   COUNT(j.Id) AS 'JourneyCount'
FROM Journeys j
JOIN Spaceports sp
ON j.DestinationSpaceportId = sp.Id
JOIN Planets p
ON sp.PlanetId = p.Id
GROUP BY p.[Name]
ORDER BY JourneyCount DESC,
		 PlanetName