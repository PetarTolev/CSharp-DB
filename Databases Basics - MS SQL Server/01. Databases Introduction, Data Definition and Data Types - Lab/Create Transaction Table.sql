CREATE TABLE Transactions (
	Id INT PRIMARY KEY IDENTITY,
	AccountId INT FOREIGN KEY REFERENCES Accounts(Id),
	OldBalance DECIMAL(15, 2) NOT NULL,
	NewBalance DECIMAL(15, 2) NOT NULL,
	Ammount AS NewBalance - OldBalance,
	[DateTime] DATETIME2
)