SELECT u.Username,
	   c.[Name] AS 'CategoryName'
FROM Reports r
JOIN Users u
ON r.UserId = u.Id
JOIN Categories c
ON r.CategoryId = c.Id
WHERE FORMAT(u.Birthdate, 'dd-MM') = FORMAT(r.OpenDate, 'dd-MM')
ORDER BY u.Username,
		 CategoryName