CREATE TABLE Categories (
	Id INT PRIMARY KEY IDENTITY,
	CategoryName NVARCHAR(50) NOT NULL,
	DailyRate DECIMAL,
	WeeklyRate DECIMAL,
	MonthlyRate DECIMAL,
	WeekendRate DECIMAL
)

CREATE TABLE Cars (
	Id INT PRIMARY KEY IDENTITY,
	PlateNumber NVARCHAR(50) NOT NULL,
	Manufacturer NVARCHAR(50) NOT NULL,
	Model NVARCHAR(50) NOT NULL,
	CarYear INT NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Doors INT NOT NULL,
	Picture VARBINARY,
	Condition NVARCHAR(50),
	Available BIT NOT NULL
)

CREATE TABLE Employees (
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Title NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Customers (
	Id INT PRIMARY KEY IDENTITY,
	DriverLicenseNumber NVARCHAR(50) NOT NULL,
	FullName NVARCHAR(50) NOT NULL,
	[Address] NVARCHAR(50) NOT NULL,
	City NVARCHAR(50) NOT NULL,
	ZIPCode INT NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE RentalOrders (
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	CoustomerId INT FOREIGN KEY REFERENCES Customers(Id) NOT NULL,
	CarId INT FOREIGN KEY REFERENCES Cars(Id) NOT NULL,
	TankLevel INT NOT NULL,
	KilometrageStart INT NOT NULL,
	KilmoetrageEnd INT NOT NULL,
	TotalKilometrage AS KilmoetrageEnd - KilometrageStart,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	TotalDays AS DATEDIFF(DAY, StartDate, EndDate),
	RateApplied DECIMAL,
	TaxRate DECIMAL,
	OrderStatus NVARCHAR(50),
	Notes NVARCHAR(MAX)
)

INSERT INTO Categories (CategoryName, DailyRate, WeeklyRate, MonthlyRate, WeekendRate) VALUES
('Mini', 12.50, 12.50, 12.50, 12.50),
('Economy', 12.50, 12.50, 12.50, 12.50),
('Compact', 12.50, 12.50, 12.50, 12.50)

INSERT INTO Cars (PlateNumber, Manufacturer, Model, CarYear, CategoryId, Doors, Picture, Condition, Available) VALUES
('A2316', 'BMW', 'M8', 2015, 1, 3, 3451, 'Good', 1),
('S2000', 'Honda', 'S2000', 2002, 2, 2, 1231, 'Excelent', 0),
('SA2015', 'Audi', 'A8', 2017, 3, 5, 2341, 'Good', 1)

INSERT INTO Employees (FirstName, LastName, Title) VALUES
('Petar', 'Tolev', 'Manager'),
('Ivan', 'Goshev', 'Driver'),
('Dimitar', 'Stoqnov', 'Logistcian')

INSERT INTO Customers (DriverLicenseNumber, FullName, [Address], City, ZIPCode) VALUES
('1256', 'Georgi Dimitrov', 'kv. Mladost', 'Burgas', 8000),
('2000', 'Martin Ivanov', 'kv. Izgrev', 'Sofiq', 1700),
('1590', 'Stoqn Gochev', 'kv. Svoboda', 'Plovdiv', 3000)

INSERT INTO RentalOrders (EmployeeId, CoustomerId, CarId, TankLevel, KilometrageStart, KilmoetrageEnd, StartDate, EndDate) VALUES
(2, 3, 3, 10, 10, 200, '2019-06-25', '2019-06-13'),
(3, 1, 2, 20, 10, 200, '2019-01-14', '2019-06-14'),
(1, 2, 1, 30, 10, 200, '2019-09-12', '2019-09-15')