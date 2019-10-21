SELECT u.Username,
	   AVG(f.Size) AS 'Size'
FROM Commits c
FULL JOIN Users u
ON c.ContributorId = u.Id
JOIN Files f
ON f.CommitId = c.Id
WHERE c.Id IS NOT NULL
GROUP BY u.Username
ORDER BY Size DESC,
		 u.Username