CREATE VIEW v_EmployeeNameJobTitle
AS
     SELECT FirstName + ' ' + ISNULL(MiddleName, '') + ' ' + LastName AS 'FullName', 
            JobTitle
     FROM Employees;