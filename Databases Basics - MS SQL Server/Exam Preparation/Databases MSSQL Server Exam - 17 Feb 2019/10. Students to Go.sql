SELECT CONCAT(s.FirstName, ' ', s.LastName) AS 'FullName'
FROM Students s
FULL JOIN StudentsExams se
ON s.Id = se.StudentId
FULL JOIN Exams e
ON se.ExamId = e.Id
WHERE e.Id IS NULL
ORDER BY FullName