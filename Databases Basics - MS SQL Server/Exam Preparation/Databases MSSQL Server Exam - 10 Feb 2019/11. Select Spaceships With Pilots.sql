SELECT DISTINCT ss.[Name],
	   ss.Manufacturer
FROM Spaceships ss
JOIN Journeys j
ON j.SpaceshipId = ss.Id
JOIN TravelCards tc
ON tc.JourneyId = j.Id
JOIN Colonists c
ON c.Id = tc.ColonistId
WHERE DATEDIFF(YEAR, c.BirthDate, '01/01/2019') < 30 AND
	tc.JobDuringJourney = 'Pilot'
ORDER BY ss.[Name]