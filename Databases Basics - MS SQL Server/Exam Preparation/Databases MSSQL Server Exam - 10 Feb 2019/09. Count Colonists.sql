SELECT COUNT(c.Id)
FROM TravelCards tc
JOIN Colonists c
ON tc.ColonistId = c.Id
JOIN Journeys j
ON tc.JourneyId = j.Id
GROUP BY j.Purpose
HAVING j.Purpose = 'Technical'