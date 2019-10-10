CREATE FUNCTION udf_ExamGradesToUpdate(@studentId INT, @grade DECIMAL(15, 2))
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @studentExist INT = 
	(
	SELECT TOP 1 s.Id
	FROM Students s
	WHERE s.Id = @studentId
	)

	IF(@studentExist IS NULL)
	BEGIN
		RETURN 'The student with provided id does not exist in the school!'
	END

	IF(@grade > 6.00)
	BEGIN
		RETURN 'Grade cannot be above 6.00!'
	END

	DECLARE @studentName NVARCHAR(20) = 
	(
		SELECT TOP 1 s.FirstName
		FROM StudentsExams se
		JOIN Students s
		ON s.Id = se.StudentId
		WHERE se.StudentId = @studentId
	)

	DECLARE @countOfGrades NVARCHAR;
	SET @countOfGrades = 
	(
		SELECT COUNT(se.Grade)
		FROM StudentsExams se
		WHERE se.Grade BETWEEN @grade AND @grade + 0.50 AND 
			  se.StudentId = @studentId
		GROUP BY se.StudentId
	)

	RETURN 'You have to update ' + @countOfgrades + ' grades for the student ' + @studentName;
END