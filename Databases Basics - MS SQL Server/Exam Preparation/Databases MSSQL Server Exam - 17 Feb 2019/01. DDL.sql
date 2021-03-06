	CREATE TABLE Students 
	(
		Id INT PRIMARY KEY IDENTITY NOT NULL,
		FirstName NVARCHAR(20) NOT NULL,
		MiddleName NVARCHAR(20),
		LastName NVARCHAR(20) NOT NULL,
		Age INT NOT NULL CHECK(Age > 0),
		[Address] NVARCHAR(30),
		Phone NVARCHAR(10)
	)

	CREATE TABLE Subjects 
	(
		Id INT PRIMARY KEY IDENTITY NOT NULL,
		[Name] NVARCHAR(20) NOT NULL,
		Lessons INT NOT NULL
	)

	CREATE TABLE StudentsSubjects 
	(
		Id INT PRIMARY KEY IDENTITY NOT NULL,
		StudentId INT FOREIGN KEY REFERENCES Students(Id) NOT NULL,
		SubjectId INT FOREIGN KEY REFERENCES Subjects(Id) NOT NULL,
		Grade DECIMAL(10, 2) NOT NULL CHECK(Grade >= 2 AND Grade <= 6)
	)

	CREATE TABLE Exams 
	(
		Id INT PRIMARY KEY IDENTITY NOT NULL,
		[Date] DATE,
		SubjectId INT FOREIGN KEY REFERENCES Subjects(Id) NOT NULL
	)

	CREATE TABLE StudentsExams 
	(
		StudentId INT FOREIGN KEY REFERENCES Students(Id) NOT NULL,
		ExamId INT FOREIGN KEY REFERENCES Exams(Id) NOT NULL,
		Grade DECIMAL(15, 2) NOT NULL CHECK(Grade >= 2 AND Grade <= 6)
	
		CONSTRAINT PK_StudentsExams PRIMARY KEY (StudentId, ExamId)
	)

	CREATE TABLE Teachers 
	(
		Id INT PRIMARY KEY IDENTITY NOT NULL,
		FirstName NVARCHAR(20) NOT NULL,
		LastName NVARCHAR(20) NOT NULL, 
		[Address] NVARCHAR(20) NOT NULL,
		Phone NVARCHAR(10),
		SubjectId INT FOREIGN KEY REFERENCES Subjects(Id) NOT NULL
	)

	CREATE TABLE StudentsTeachers 
	(
		StudentId INT FOREIGN KEY REFERENCES Students(Id) NOT NULL,
		TeacherId INT FOREIGN KEY REFERENCES Teachers(Id) NOT NULL
	
		CONSTRAINT PK_StudentsTeachers PRIMARY KEY (StudentId, TeacherId)
	)