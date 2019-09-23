SELECT PeakName, RiverName,  CONCAT(LOWER(PeakName), LOWER(SUBSTRING(RiverName, 2, LEN(RiverName)))) AS 'Mix'
FROM Peaks, Rivers
WHERE RIGHT(PeakName, 1) = Left(RiverName, 1)
ORDER BY Mix