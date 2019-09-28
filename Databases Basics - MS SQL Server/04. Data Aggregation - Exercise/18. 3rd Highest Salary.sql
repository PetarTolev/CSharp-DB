SELECT DISTINCT e.DepartmentID, e.Salary
  FROM (
SELECT DepartmentID, Salary, DENSE_RANK() OVER (PARTITION BY DepartmentID ORDER BY Salary DESC) AS SalaryRank 
  FROM Employees) AS e
 WHERE e.SalaryRank = 3