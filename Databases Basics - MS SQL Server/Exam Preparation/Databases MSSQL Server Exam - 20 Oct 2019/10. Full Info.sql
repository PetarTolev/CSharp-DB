SELECT 
	   CASE
		   WHEN e.FirstName IS NULL THEN 'None'
		   ELSE CONCAT(e.FirstName, ' ', e.LastName)
	   END AS 'Employee',
	   CASE
		   WHEN d.[Name] IS NULL THEN 'None'
	   	   ELSE  d.[Name] 
	   END AS 'Department',
	   c.[Name] AS 'Category',
	   r.[Description] AS 'Description',
	   FORMAT(r.OpenDate, 'dd.MM.yyyy') AS 'OpenDate',
	   s.Label AS 'Status',
	   u.[Name] AS 'User'
FROM Reports r
FULL JOIN Employees e
ON r.EmployeeId = e.Id
FULL JOIN Departments d
ON e.DepartmentId = d.Id
FULL JOIN Categories c
ON r.CategoryId = c.Id
FULL JOIN [Status] s
ON r.StatusId = s.Id
JOIN Users u
ON r.UserId = u.Id
ORDER BY e.FirstName DESC,
		 e.LastName DESC,
		 Department,
		 Category,
		 [Description],
		 OpenDate,
		 [Status],
		 [User]