CREATE PROC usp_GetHoldersWithBalanceHigherThan @Amount DECIMAL(18, 4)
AS
BEGIN
	SELECT ah.FirstName, ah.LastName
	FROM Accounts a
	JOIN AccountHolders ah
	ON a.AccountHolderId = ah.Id
	GROUP BY ah.FirstName, ah.LastName
	HAVING SUM(a.Balance) > @Amount
	ORDER BY ah.FirstName,
			 ah.LastName
END


