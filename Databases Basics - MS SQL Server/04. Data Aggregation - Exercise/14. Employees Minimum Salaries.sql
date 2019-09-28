SELECT e.DepartmentID,
	   MIN(e.Salary) AS [MinimumSlary]
FROM Employees e
WHERE e.HireDate > '01-01-2000' AND
	  e.DepartmentID IN(2, 5, 7)
GROUP BY e.DepartmentID
