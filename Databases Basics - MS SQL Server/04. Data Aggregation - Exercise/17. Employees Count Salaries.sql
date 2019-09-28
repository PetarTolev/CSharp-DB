SELECT COUNT(e.Salary) AS [Count]
FROM Employees e
WHERE e.ManagerID IS NULL