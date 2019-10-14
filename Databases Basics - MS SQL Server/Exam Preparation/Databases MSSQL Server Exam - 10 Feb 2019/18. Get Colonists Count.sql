CREATE FUNCTION udf_GetColonistsCount(@planetName VARCHAR(30))
RETURNS INT
BEGIN
	RETURN (SELECT COUNT(*)
	FROM Journeys j
	JOIN TravelCards tc
	ON j.Id = tc.JourneyId
	JOIN Colonists c
	ON tc.ColonistId = c.Id
	JOIN Spaceports s
	ON j.DestinationSpaceportId = s.Id
	JOIN Planets p
	ON s.PlanetId = p.Id
	WHERE p.[Name] = @planetName)
END