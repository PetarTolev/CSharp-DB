  SELECT TOP 10
           e.FirstName,
           e.LastName,
           e.DepartmentID
    FROM Employees e
   WHERE e.Salary > (SELECT AVG(em.Salary)
                         FROM Employees em
                        WHERE e.DepartmentID = em.DepartmentID)
ORDER BY e.DepartmentID