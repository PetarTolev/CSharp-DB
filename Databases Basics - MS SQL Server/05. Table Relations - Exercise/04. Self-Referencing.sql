CREATE TABLE Teachers (
	TeacherID INT IDENTITY(101, 1),
	[Name] NVARCHAR(50) NOT NULL,
	ManagerID INT NOT NULL,

	CONSTRAINT PK_Teachers_TeacherID 
	PRIMARY KEY (TeacherID),
	CONSTRAINT FK_Teachers_ManagerID 
	FOREIGN KEY (ManagerID) REFERENCES Teachers(TeacherID)
)