CREATE FUNCTION ufn_CalculateFutureValue (@sum DECIMAL(18, 4), @yearlyInterestRate FLOAT, @numberOfYears INT)
RETURNS DECIMAL(18, 4)
AS
BEGIN
	DECLARE @result DECIMAL(18, 4)= @sum * POWER((1 + (1 * @yearlyInterestRate)), @numberOfYears)
	RETURN ROUND(@result, 4) 
END