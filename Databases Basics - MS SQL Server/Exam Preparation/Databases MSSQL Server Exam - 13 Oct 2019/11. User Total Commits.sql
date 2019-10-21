CREATE FUNCTION udf_UserTotalCommits(@username NVARCHAR(50))
RETURNS INT
AS
BEGIN
	RETURN
	(
		SELECT COUNT(*)
		FROM Commits c
		JOIN Users u
		ON c.ContributorId = u.Id
		WHERE u.Username = @username
	)
END
