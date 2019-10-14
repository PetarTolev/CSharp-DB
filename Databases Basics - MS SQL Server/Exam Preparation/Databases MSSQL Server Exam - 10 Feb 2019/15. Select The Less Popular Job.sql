SELECT TOP(1) tc.JourneyId, tc.JobDuringJourney AS JobName
      FROM TravelCards AS tc
     WHERE tc.JourneyId = (SELECT TOP(1) j.Id FROM Journeys AS j ORDER BY DATEDIFF(MINUTE, j.JourneyStart, j.JourneyEnd) DESC)
  GROUP BY tc.JobDuringJourney, tc.JourneyId
  ORDER BY COUNT(tc.JobDuringJourney) ASC