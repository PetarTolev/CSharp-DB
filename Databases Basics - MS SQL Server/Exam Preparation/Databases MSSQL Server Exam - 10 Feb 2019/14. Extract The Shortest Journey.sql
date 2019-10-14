SELECT TOP 1 j.Id,
	   p.[Name] AS 'PlanetName',
	   sp.[Name] AS 'SpaceportName',
	   j.Purpose AS 'JourneyPurpose'
FROM Journeys j
JOIN Spaceports sp
ON sp.Id = j.DestinationSpaceportId
JOIN Planets p
ON sp.PlanetId = p.Id
ORDER BY DATEDIFF(SECOND, j.JourneyStart, j.JourneyEnd) ASC