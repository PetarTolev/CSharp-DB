CREATE TABLE Logs (
	LogId INT PRIMARY KEY IDENTITY,
	AccountId INT FOREIGN KEY REFERENCES Accounts(Id),
	OldSum DECIMAL(15, 2),
	NewSum DECIMAL(15, 2)
)

CREATE TRIGGER ChangeSumFromAccounts
ON Accounts
AFTER UPDATE
AS
BEGIN
	INSERT INTO Logs(AccountId, OldSum, NewSum)
	SELECT i.AccountHolderId,
		   d.Balance,
		   i.Balance
	FROM inserted i
	INNER JOIN deleted d
	ON d.AccountHolderId = i.AccountHolderId
END