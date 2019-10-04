SELECT c.CountryCode, COUNT(c.CountryCode)  'MountainRanges'
FROM Countries AS c
JOIN MountainsCountries AS mc
ON mc.CountryCode = c.CountryCode
JOIN Mountains AS m
ON m.Id = mc.MountainId
WHERE c.CountryCode IN ('US', 'RU', 'BG')
GROUP BY c.CountryCode