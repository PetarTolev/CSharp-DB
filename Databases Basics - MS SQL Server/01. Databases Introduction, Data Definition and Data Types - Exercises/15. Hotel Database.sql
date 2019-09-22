CREATE TABLE Employees (
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50),
	LastName NVARCHAR(50),
	Title NVARCHAR(50),
	Notes NVARCHAR(MAX)
)

CREATE TABLE Customers (
	AccountNumber INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	PhoneNumber NVARCHAR(20) NOT NULL,
	EmergencyName NVARCHAR(50),
	EmergencyNumber NVARCHAR(20) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE RoomStatus (
	RoomStatus NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE RoomTypes (
	RoomType NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE BedTypes (
	BedType NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Rooms (
	RoomNumber INT PRIMARY KEY IDENTITY,
	RoomType NVARCHAR(50) FOREIGN KEY REFERENCES RoomTypes(RoomType),
	BedType NVARCHAR(50) FOREIGN KEY REFERENCES BedTypes(BedType),
	Rate DECIMAL(9, 2),
	RoomStatus NVARCHAR(50) FOREIGN KEY REFERENCES RoomStatus(RoomStatus),
	Notes NVARCHAR(MAX)
)

CREATE TABLE Payments (
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Customers(AccountNumber),
	PaymentDate	DATE NOT NULL,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber),
	FirstDateOccupied DATE NOT NULL,
	LastDateOccupied DATE NOT NULL,
	TotalDays AS DATEDIFF(DAY, LastDateOccupied, FirstDateOccupied),
	AmmountCharged DECIMAL(9, 2) NOT NULL,
	TaxRate DECIMAL(9, 2),
	TaxAmmount DECIMAL(9, 2),
	PaymentTotal DECIMAL(9, 2),
	Notes NVARCHAR(MAX)
)

CREATE TABLE Occupancies (
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
	DateOccupancied DATE,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber),
	RoomNumber INT FOREIGN KEY REFERENCES Rooms(RoomNumber),
	RateApplied DECIMAL(9, 2) NOT NULL,
	PhoneCharge DECIMAL(9, 2),
	Notes NVARCHAR(MAX)
)

INSERT INTO Employees (FirstName, LastName, Title, Notes) VALUES 
('Ivan', 'Ivanov', 'Manager', 'I am Ivan'),
('Stoqn', 'Dimitrov', 'Ðeceptionist', 'I am Stoqn'),
('Georgi', 'Todorov', 'Ìaid', 'I am Georgi')

INSERT INTO Customers(FirstName, LastName, PhoneNumber, EmergencyNumber) VALUES
('Petar', 'Georgiev', '0895751195', '123'),
('Hristo', 'Dimitrov', '0895151617', '123'),
('Ivailo', 'Subev', '0892121829', '123')

INSERT INTO RoomStatus (RoomStatus, Notes) VALUES
('Clean', 'Room is clean'),
('Dirty', 'Room is dirty'),
('Repair', 'Room is for repair')

INSERT INTO RoomTypes (RoomType, Notes) VALUES
('Large', 'Room'),
('Small', 'Room'),
('Medium', 'Room')

INSERT INTO BedTypes (BedType) VALUES 
('Normal'),
('Comfort'),
('VIP')

INSERT INTO Rooms (RoomType, BedType, Rate, RoomStatus) VALUES 
('Large', 'VIP', 55.60, 'Dirty'),
('Small', 'Normal', 22.50, 'Clean'),
('Medium', 'Comfort', 35.20, 'Repair')

INSERT INTO Payments (EmployeeId, PaymentDate, AccountNumber, FirstDateOccupied, LastDateOccupied, AmmountCharged, TaxRate) VALUES 
(1, '2015-10-12', 1, '2015-10-01', '2015-10-11', 1242.12, 253.12),
(2, '2016-11-16', 2, '2016-11-03', '2016-11-15', 2412.12, 163.12),
(3, '2017-12-18', 3, '2017-12-05', '2017-12-17', 1712.12, 613.12)

INSERT INTO Occupancies (EmployeeId, AccountNumber, RoomNumber, RateApplied) VALUES 
(1, 1, 1, 33.3),
(2, 2, 2, 44.4),
(3, 3, 3, 55.5)
