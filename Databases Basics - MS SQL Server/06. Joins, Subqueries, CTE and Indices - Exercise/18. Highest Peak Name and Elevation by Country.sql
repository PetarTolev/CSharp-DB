SELECT TOP 5 c.CountryName AS 'Country',
	   ISNULL(p.PeakName, '(no highest peak)') AS 'Highest Peak Name', 
	   ISNULL(p.Elevation, '0') AS 'Highest Peak Elevation',
	   ISNULL(m.MountainRange, '(no mountain)') AS 'Mountain'
FROM Countries c
LEFT JOIN MountainsCountries mc
ON mc.CountryCode = c.CountryCode
LEFT JOIN Mountains m
ON m.Id = mc.MountainId
LEFT JOIN Peaks p
ON p.MountainId = m.Id
ORDER BY c.CountryName,
		 p.PeakName