CREATE TABLE People (
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(200) NOT NULL,
	Picture VARBINARY(MAX),
	Height DECIMAL(5, 2),
	[Weight] DECIMAL(5, 2),
	Gender CHAR(1) NOT NULL,
	Birthdate DATE NOT NULL,
	Biography NVARCHAR(MAX)
)

INSERT INTO People ([Name], Picture, Height, [Weight], Gender, Birthdate, Biography) VALUES
('Ivan', 1000, 1.7999999, 100.001, 'm', '1996-05-12', 'I am Ivan'),
('Georgi', NULL, 1.95, 67.300, 'm', '2002-04-09', 'I am Georgi'),
('Mariq', 1234, 1.66, 50, 'f', '1970-12-12', 'I am Mariq'),
('Simona', 2002, 1.60, 47, 'f', '1999-09-07', 'I am Simona'),
('Dimitar', 5555, 1.7333, 80, 'm', '1998-03-15', 'I am Dimitar')
