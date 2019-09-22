CREATE TABLE Directors (
	Id INT PRIMARY KEY IDENTITY,
	DirectorName NVARCHAR(50) NOT NULL,
	Notes TEXT
)

CREATE TABLE Genres (
	Id INT PRIMARY KEY IDENTITY,
	GenreName NVARCHAR(50) NOT NULL,
	Notes TEXT
)

CREATE TABLE Categories (
	Id INT PRIMARY KEY IDENTITY,
	CategoryName NVARCHAR(50) NOT NULL,
	Notes TEXT
)

CREATE TABLE Movies (
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(50) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(Id),
	CopyrightYear DATE NOT NULL,
	[Length] INT NOT NULL,
	GenreId INT FOREIGN KEY REFERENCES Genres(Id),
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Rating DECIMAL(2,1),
	Notes TEXT
)

INSERT INTO Directors (DirectorName, Notes) VALUES 
('Steven Spielberg', 'One of the most influential personalities in the history of cinema.'),
('Martin Scorsese', NULL),
('Quentin Tarantino', NULL),
('Orson Welles', 'His father was a well-to-do inventor.'),
('Ridley Scott', NULL)

INSERT INTO Genres (GenreName, Notes) VALUES 
('Action', NULL),
('Biograpy', 'A biographical film is a film that dramatizes the life.'),
('Comedy', 'A comedy film is a genre of film in which the main emphasis is on humour.'),
('Adventure', NULL),
('Drama', 'Drama is a genre of narrative fiction.')

INSERT INTO Categories (CategoryName, Notes) VALUES
('Action-Drama', NULL),
('Biography-Drama', NULL),
('Comedy-Drama', NULL),
('Adventure-Fantasy', NULL),
('Drama-Thriller', NULL)

INSERT INTO Movies (Title, DirectorId, CopyrightYear, [Length], GenreId, CategoryId, Rating, Notes) VALUES
('West Side Story', 1, '1998-02-11', 108, 1, 1, 5.8, NULL),
('Roosevelt', 2, '2000-03-15', 92, 2, 2, 9.7, NULL),
('Untitled Star Trek Project', 3, '2019-12-1', 131, 3, 3, 6.6, NULL),
('The Other Side of the Wind', 4, '2005-07-09', 70, 4, 4, NULL, 'A Hollywood director emerges from semi-exile with plans to complete work on an innovative motion picture.'),
('Battle of Britain', 5, '1968-06-01', 85, 5, 5, 7.5, NULL)