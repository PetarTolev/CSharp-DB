CREATE PROC usp_ExcludeFromSchool(@StudentId INT)
AS
BEGIN
	DECLARE @gettedStudentId INT = (
								SELECT s.Id
								FROM Students s
								WHERE s.Id = @StudentId
	)

	IF(@gettedStudentId IS NULL)
	BEGIN
		RAISERROR('This school has no student with the provided id!', 16, 1)
	END

	DELETE FROM StudentsExams
	WHERE StudentId = @StudentId

	DELETE FROM StudentsSubjects
	WHERE StudentId = @StudentId

	DELETE FROM StudentsTeachers
	WHERE StudentId = @StudentId

	DELETE FROM Students
	WHERE Id = @StudentId
END