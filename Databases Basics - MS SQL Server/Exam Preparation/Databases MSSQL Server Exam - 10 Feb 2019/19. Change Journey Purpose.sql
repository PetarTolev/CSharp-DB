CREATE PROC usp_ChangeJourneyPurpose(@journeyId INT, @newPurpose VARCHAR(20))
AS
BEGIN
	DECLARE @CurrentJourneyId INT;
	SET @CurrentJourneyId = (
		SELECT Id
		FROM Journeys
		WHERE Id = @journeyId
	)

	IF(@CurrentJourneyId IS NULL)
	BEGIN
		;THROW 50001, 'The journey does not exist!', 1
	END

	DECLARE @CurrentPurpose VARCHAR(20);
	SET @CurrentPurpose = (
		SELECT Purpose
		FROM Journeys
		WHERE Id = @journeyId
	)

	IF(@CurrentPurpose = @newPurpose)
	BEGIN
		;THROW 50002, 'You cannot change the purpose!', 2
	END

	UPDATE Journeys
	SET Purpose = @newPurpose
	WHERE Id = @journeyId
END