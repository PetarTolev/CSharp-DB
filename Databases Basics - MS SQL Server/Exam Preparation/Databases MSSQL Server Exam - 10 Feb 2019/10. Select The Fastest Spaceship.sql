SELECT TOP 1  ss.[Name],
			  sp.[Name]
FROM Journeys j
JOIN Spaceports sp
ON j.DestinationSpaceportId = sp.Id
JOIN Spaceships ss
ON j.SpaceshipId = ss.Id
ORDER BY ss.LightSpeedRate DESC