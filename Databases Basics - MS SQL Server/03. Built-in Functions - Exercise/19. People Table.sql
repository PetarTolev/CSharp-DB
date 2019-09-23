CREATE TABLE People(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50),
	Birthdate DATETIME
)

INSERT INTO People([Name], Birthdate) VALUES
('Victor', '2000-12-07 00:00:00.000'),
('Steven', '1992-09-10 00:00:00.000'),
('Stephen', '1910-09-19 00:00:00.000'),
('John', '2010-01-06 00:00:00.000')

SELECT [Name],
YEAR(GETDATE()) - YEAR(Birthdate) AS 'Age in Year',
(YEAR(GETDATE()) - YEAR(Birthdate)) * 12 AS 'Age in Months',
(YEAR(GETDATE()) - YEAR(Birthdate)) * 12 * 30 AS 'Age in Days',
(YEAR(GETDATE()) - YEAR(Birthdate)) * 12 * 30 * 24 * 60 AS 'Age in Minutes'
FROM People