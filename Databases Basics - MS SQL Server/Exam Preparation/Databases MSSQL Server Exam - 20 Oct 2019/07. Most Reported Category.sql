SELECT TOP 5 c.[Name],
	   COUNT(r.CategoryId) 'ReportsNumber'
FROM Reports r
JOIN Categories c
ON r.CategoryId = c.Id
GROUP BY c.[Name]
ORDER BY ReportsNumber DESC,
		 c.[Name]