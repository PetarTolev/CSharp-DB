CREATE PROC p_Withdraw @AccountId INT, @Ammount DECIMAL(15, 2) AS
BEGIN
	DECLARE @OldBalance DECIMAL(15, 2)
	SELECT @OldBalance = Balance FROM Accounts WHERE Id = @AccountId
	IF (@OldBalance - @Ammount >= 0)
	BEGIN 
		UPDATE Accounts
		SET Balance -= @Ammount
		WHERE Id = @AccountId
	END
	ELSE
	BEGIN
		RAISERROR('Insufficated fund', 10, 1)
	END
END