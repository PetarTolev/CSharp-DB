SELECT s.[Name],
	   AVG(ss.Grade) AS 'AverageGrade'
FROM Subjects s 
JOIN StudentsSubjects ss ON ss.SubjectId = s.Id
GROUP BY s.[Name], s.Id
ORDER BY s.Id