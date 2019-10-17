CREATE PROC usp_TransferMoney(@SenderId INT, @RecieverId INT, @Amount DECIMAL(15, 4))
AS
IF(@Amount > 0)
BEGIN
	UPDATE Accounts
	SET Balance -= @Amount
	WHERE Id = @SenderId

	UPDATE Accounts
	SET Balance += @Amount
	WHERE Id = @RecieverId
END