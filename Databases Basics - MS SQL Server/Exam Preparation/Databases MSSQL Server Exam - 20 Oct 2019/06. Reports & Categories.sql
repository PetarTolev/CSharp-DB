SELECT r.[Description],
	   c.[Name]
FROM Reports r
JOIN Categories c
ON r.CategoryId = c.Id
WHERE CategoryId IS NOT NULL
ORDER BY r.[Description],
		 c.[Name]