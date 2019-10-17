CREATE FUNCTION ufn_CashInUsersGames(@gameName NVARCHAR(50))
RETURNS TABLE
	RETURN (SELECT SUM(t.Cash) AS SumCash
			FROM (
				SELECT ug.Cash, 
					   ROW_NUMBER() OVER (ORDER BY ug.Cash DESC) AS RowNumber
				FROM UsersGames ug
				JOIN Games g 
				ON g.Id = ug.GameId
				WHERE g.[Name] = @gameName
			) AS t
			WHERE t.RowNumber % 2 = 1)