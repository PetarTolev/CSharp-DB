SELECT CONCAT(e.FirstName, ' ', e.LastName) AS 'FullName',
	   COUNT(u.Id) AS 'UserCout'
FROM Reports r
JOIN Users u
ON r.UserId = u.id
RIGHT JOIN Employees e
ON e.Id = r.EmployeeId
GROUP BY e.FirstName,
		 e.LastName
ORDER BY UserCout DESC,
		 FullName