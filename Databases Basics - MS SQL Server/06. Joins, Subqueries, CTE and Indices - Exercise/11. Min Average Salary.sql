SELECT MIN(DepartmentAverageSalary) AS 'MinAverageSalary'
FROM (SELECT AVG(e.Salary) AS 'DepartmentAverageSalary'
FROM Employees AS e
GROUP BY e.DepartmentID) AS t