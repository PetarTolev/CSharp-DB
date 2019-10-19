SELECT s.FirstName + ' ' + ISNULL(s.MiddleName + ' ', '') + s.LastName AS 'FullName'
FROM Students s
FULL JOIN StudentsSubjects ss
ON s.Id = ss.StudentId
LEFT JOIN Subjects su
ON ss.SubjectId = su.Id
WHERE ss.Id IS NULL
ORDER BY s.FirstName