SELECT FirstName, LastName 
FROM Employees
WHERE CHARINDEX('ei', LastName, 1) > 0