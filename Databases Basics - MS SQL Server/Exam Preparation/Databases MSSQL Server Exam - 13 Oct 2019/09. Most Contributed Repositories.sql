SELECT TOP(5) r.Id,
			  r.[Name],
			  COUNT(c.Id) AS 'Commits'
FROM Repositories r
JOIN RepositoriesContributors rc
ON rc.RepositoryId = r.Id
JOIN Users u
ON u.Id = rc.ContributorId
JOIN Commits c
ON c.RepositoryId = r.Id
GROUP BY r.Id,
		 r.[Name]
ORDER BY Commits DESC,
		 r.Id,
		 r.[Name]