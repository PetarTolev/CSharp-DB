SELECT e.FirstName,
	   e.LastName,
	   e.HireDate,
	   d.[Name] AS 'DeptName'
FROM Employees AS e
JOIN Departments AS d
ON e.DepartmentID = d.DepartmentID
WHERE d.DepartmentID IN (3, 10) AND
	  e.HireDate > '1.1.1999'
ORDER BY e.HireDate