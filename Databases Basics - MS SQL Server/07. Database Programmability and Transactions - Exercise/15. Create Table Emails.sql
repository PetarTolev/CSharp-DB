CREATE TABLE NotificationEmails (
	Id INT PRIMARY KEY IDENTITY,
	Recipient INT,
	[Subject] NVARCHAR(MAX),
	Body NVARCHAR(MAX)
)

CREATE TRIGGER tr_LogEmail
ON Logs
AFTER INSERT
AS
BEGIN
	INSERT INTO NotificationEmails(Recipient, [Subject], Body)
	SELECT i.AccountId,
		   CONCAT('Balance change for account: ', i.AccountId),
		   CONCAT('On ', GETDATE(), ' your balance was changed from ', i.OldSum, ' to ', i.NewSum)
	FROM inserted i
END