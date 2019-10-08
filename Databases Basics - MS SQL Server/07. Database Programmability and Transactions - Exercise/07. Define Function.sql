CREATE FUNCTION ufn_IsWordComprised (@setOfLetters NVARCHAR(50), @word NVARCHAR(50))
RETURNS BIT
AS
BEGIN
	DECLARE @i INT = 1;
	WHILE (@i <= LEN(@word))
	BEGIN
		DECLARE @Symbol NVARCHAR(1) = SUBSTRING(@word, @i, 1);

		IF (CHARINDEX(@Symbol, @setOfLetters, 1) = 0)
		BEGIN
			RETURN 0;
		END
		SET @i += 1
	END

	RETURN 1;
END